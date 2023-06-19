using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using DynaMight.BatchWrapper;
using DynaMight.Builders;
using DynaMight.Converters;
using DynaMight.Pagination;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DynaMight;

/// <summary>
/// DynaMightContext is a "wrapper" around the DynamoDBContext. It implements some wrappers for batches, as some
/// definitions for common methods, like the `GetAll`, `GetPage`, `GetFilteredPage` and `ExecuteAtomicOperation`.
/// </summary>
[ExcludeFromCodeCoverage]
public class DynaMightContext : DynamoDBContext, IDynaMightContext
{
    private readonly AmazonDynamoDBClient _client;
    private readonly DynamoDBContextConfig _config;

    public DynaMightContext(AmazonDynamoDBClient client, DynamoDBContextConfig config) : base(client, config)
    {
        _client = client;
        _config = config;
    }

    /// <summary>
    /// Creates a DynamoDB BatchGet
    /// </summary>
    /// <typeparam name="T">Mapped type to DynamoDB item</typeparam>
    /// <returns>DynamoDB BatchGet</returns>
    public new IBatchGet<T> CreateBatchGet<T>()
        => new DefaultBatchGet<T>(base.CreateBatchGet<T>());

    /// <summary>
    /// Creates a DynamoDB BatchWrite
    /// </summary>
    /// <typeparam name="T">Mapped type to DynamoDB item</typeparam>
    /// <returns>DynamoDB BatchWrite</returns>
    public new IBatchWrite<T> CreateBatchWrite<T>()
        => new DefaultBatchWrite<T>(base.CreateBatchWrite<T>());

    /// <summary>
    /// Creates a DynamoDB MultiTable BatchGet
    /// </summary>
    /// <returns>DynamoDB MultiTable BatchGet</returns>
    public IMultiTableBatchGet CreateMultiTableBatchGet(params IBatchGet[] batches)
        => new DefaultMultiTableBatchGet(base.CreateMultiTableBatchGet(), batches);

    /// <summary>
    /// Creates a DynamoDB MultiTable BatchWrite
    /// </summary>
    /// <returns>DynamoDB MultiTable BatchWrite</returns>
    public IMultiTableBatchWrite CreateMultiTableBatchWrite(params IBatchWrite[] batches)
        => new DefaultMultiTableBatchWrite(base.CreateMultiTableBatchWrite(), batches);

    /// <summary>
    /// Executes an atomic operation, based on the AtomicBuilder passed.
    /// </summary>
    /// <param name="requestBuilder">The AtomicBuilder defines which operations will be executed and which filters
    /// /criteria will be used.</param>
    /// <param name="cancellationToken">The Cancellation Token for the operation</param>
    /// <typeparam name="T">Mapped type to DynamoDB item</typeparam>
    /// <returns>The item that was updated in the DynamoDB</returns>
    public async Task<T> ExecuteAtomicOperation<T>(IAtomicBuilder requestBuilder,
        CancellationToken cancellationToken) where T : new()
    {
        var request = requestBuilder.SetTableName<T>(_config).Build();
        var response = await _client.UpdateItemAsync(request, cancellationToken);
        return DynamoValueConverter.To<T>(response.Attributes);
    }

    /// <summary>
    /// Get all the registers in DynamoDB that fulfill the query builder
    /// </summary>
    /// <param name="queryBuilder">The QueryBuilder defines which filters/criteria will be used.</param>
    /// <param name="cancellationToken">The Cancellation Token for the operation</param>
    /// <typeparam name="T">Mapped type to DynamoDB item</typeparam>
    /// <returns>The list of items that fulfill the conditions from the query</returns>
    public async Task<List<T>> GetAll<T>(IQueryBuilder queryBuilder, CancellationToken cancellationToken)
    {
        var queryConfig = queryBuilder.Build(true);
        var search = FromQueryAsync<T>(queryConfig);
        return await search.GetRemainingAsync(cancellationToken);
    }

    /// <summary>
    /// Get all the registers in DynamoDB that fulfill the query builder, based on a page definition. Filters/criteria
    /// are not used by this method. If you want to do filtered page, use the method <see cref="GetFilteredPage{T}"/>.
    /// </summary>
    /// <param name="queryBuilder">The QueryBuilder defines a `pageToken` to have pagination. An empty `pageToken` will
    /// get the first page.</param>
    /// <param name="cancellationToken">The Cancellation Token for the operation</param>
    /// <typeparam name="T">Mapped type to DynamoDB item</typeparam>
    /// <returns>The list of items from the paged query</returns>
    public async Task<Page<T>> GetPage<T>(IQueryBuilder queryBuilder, CancellationToken cancellationToken)
    {
        var queryConfig = queryBuilder.Build(false);
        var asyncSearch = FromQueryAsync<T>(queryConfig);
        var items = await asyncSearch.GetNextSetAsync(cancellationToken);
        return items?.Any() ?? false ? new Page<T>(GetPaginationToken(asyncSearch), items) : Page<T>.Empty();
    }

    /// <summary>
    /// Get all the registers in DynamoDB that fulfill the query builder, based on a page definition and the
    /// filters/criteria.
    /// </summary>
    /// <param name="queryBuilder">The QueryBuilder defines a `pageToken` to have pagination. An empty `pageToken` will
    /// get the first page. It defines the filters/criteria too, which will be used in this method.</param>
    /// <param name="cancellationToken">The Cancellation Token for the operation</param>
    /// <typeparam name="T">Mapped type to DynamoDB item</typeparam>
    /// <returns>The list of items from the paged/filtered query</returns>
    /// <remarks>
    /// DynamoDB does not provide filters on the query itself. This means that the query will only do the pagination and
    /// the filter by the Keys. With all the results IN MEMORY, the DynamoDB SDK does the filter. Because of this
    /// behaviour, this method can do several calls to the DynamoDB, until it fulfill the number of items desired or it
    /// reaches the end of the table. It is highly recommended that you create indexes to have a better
    /// filter for your table, using the filtered for "simple" conditions. And you should really avoid this method if
    /// in cases that you will be using this recurrently. Keep in mind that the usage of this method can increase the
    /// charges in AWS, as it can read the table from beginning to end. 
    /// </remarks>
    public async Task<Page<T>> GetFilteredPage<T>(IQueryBuilder queryBuilder, CancellationToken cancellationToken)
    {
        var queryConfig = queryBuilder.Build(true);

        var asyncSearch = FromQueryAsync<T>(queryConfig);
        var items = new List<T>();
        string? pageTokenModel = null;
        while (items.Count < queryConfig.Limit && !asyncSearch.IsDone)
        {
            items.AddRange(await asyncSearch.GetNextSetAsync(cancellationToken));
            pageTokenModel ??= GetPaginationToken(asyncSearch);
        }

        if (items.Count <= queryConfig.Limit && asyncSearch.IsDone)
            pageTokenModel = null;

        if (!items.Any())
            return Page<T>.Empty();

        return items.Count < queryConfig.Limit
            ? new Page<T>(GetPaginationToken(asyncSearch), items)
            : Crop(pageTokenModel, items, queryConfig.Limit);
    }

    //https://github.com/aws/aws-sdk-net/issues/671
    private static string GetPaginationToken<T>(AsyncSearch<T> asyncSearch)
    {
        var paginationToken = string.Empty;
        if (asyncSearch.IsDone)
            return paginationToken;

        var searchProperty = asyncSearch.GetType()
            .GetProperty("DocumentSearch", BindingFlags.NonPublic | BindingFlags.Instance);
        var searchGetter = searchProperty?.GetGetMethod(nonPublic: true);
        var search = (Search)searchGetter?.Invoke(asyncSearch, null)!;
        paginationToken = search.PaginationToken;

        return paginationToken;
    }

    private static Page<T> Crop<T>(string? pageTokenModel, List<T> items, int limit)
    {
        if (string.IsNullOrEmpty(pageTokenModel))
            return new Page<T>(string.Empty, items);

        var newList = items.Take(limit).ToList();
        return new Page<T>(CreatePageToken(pageTokenModel, newList.Last()), newList);
    }

    private static string CreatePageToken<T>(string pageTokenModel, T last)
    {
        var jsonToken = JObject.Parse(pageTokenModel);
        var jsonLast = JsonConvert.SerializeObject(last);
        var parsedLast = JObject.Parse(jsonLast);
        foreach (var key in jsonToken.Properties())
        {
            foreach (var child in key.Value.Children().OfType<JProperty>())
            {
                var newValue = parsedLast[key.Name];
                if (newValue is not null)
                    child.Value = newValue;
            }
        }

        return jsonToken.ToString(Formatting.None);
    }
}
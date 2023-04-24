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

namespace DynaMight
{
    public class DynaMightContext : DynamoDBContext, IDynaMightContext
    {
        private readonly AmazonDynamoDBClient _client;
        private readonly DynamoDBContextConfig _config;

        public DynaMightContext(AmazonDynamoDBClient client, DynamoDBContextConfig config) : base(client, config)
        {
            _client = client;
            _config = config;
        }

        public new IBatchGet<T> CreateBatchGet<T>()
            => new DefaultBatchGet<T>(base.CreateBatchGet<T>());

        public new IBatchWrite<T> CreateBatchWrite<T>()
            => new DefaultBatchWrite<T>(base.CreateBatchWrite<T>());

        public IMultiTableBatchGet CreateMultiTableBatchGet(params IBatchGet[] batches)
            => new DefaultMultiTableBatchGet(base.CreateMultiTableBatchGet(), batches);

        public IMultiTableBatchWrite CreateMultiTableBatchWrite(params IBatchWrite[] batches)
            => new DefaultMultiTableBatchWrite(base.CreateMultiTableBatchWrite(), batches);

        public async Task<T> ExecuteAtomicOperation<T>(IAtomicBuilder requestBuilder,
            CancellationToken cancellationToken) where T : new()
        {
            var request = requestBuilder.SetTableName<T>(_config).Build();
            var response = await _client.UpdateItemAsync(request, cancellationToken);
            return AttributeValueDictionaryConverter.ConvertFrom<T>(response.Attributes);
        }

        public async Task<List<T>> GetAll<T>(IQueryBuilder queryBuilder, CancellationToken cancellationToken)
        {
            var queryConfig = queryBuilder.Build(true);
            var search = FromQueryAsync<T>(queryConfig);
            return await search.GetRemainingAsync(cancellationToken);
        }

        public async Task<Page<T>> GetPage<T>(IQueryBuilder queryBuilder, CancellationToken cancellationToken)
        {
            var queryConfig = queryBuilder.Build(false);
            var asyncSearch = FromQueryAsync<T>(queryConfig);
            var items = await asyncSearch.GetNextSetAsync(cancellationToken);
            return items?.Any() ?? false ? new Page<T>(GetPaginationToken(asyncSearch), items) : Page<T>.Empty();
        }

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
}
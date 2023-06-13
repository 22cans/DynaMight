using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using DynaMight.Criteria;

namespace DynaMight.Builders;

/// <summary>
/// Defines the Builder for the Query Operation
/// </summary>
public class QueryBuilder : DynamoBuilder, IQueryBuilder
{
    private int _pageSize = IQueryBuilder.DefaultPageSize;
    private string? _pageToken;
    private bool _sortDescending;
    private string? _indexName;

    /// <summary>
    /// Creates a new Query Builder
    /// </summary>
    private QueryBuilder()
    {
    }

    /// <summary>
    /// Creates a new Query Builder
    /// </summary>
    public static IQueryBuilder Create()
        => new QueryBuilder();

    /// <summary>
    /// Adds a new Criteria into the builder
    /// </summary>
    /// <param name="condition">The condition that needs to be satisfied to add the criteria</param>
    /// <param name="func">The criteria that will be added into the builder. It uses a Func, which will be normally a
    /// `() => criteria`.</param>
    /// <returns>The QueryBuilder (for a fluent API usage)</returns>
    IQueryBuilder IQueryBuilder.AddCriteria(bool condition, Func<IDynamoCriteria> func)
    {
        base.AddCriteria(condition, func);
        return this;
    }

    /// <summary>
    /// Adds a new Criteria into the builder
    /// </summary>
    /// <param name="criteria">The criteria that will be added into the builder.</param>
    /// <returns>The QueryBuilder (for a fluent API usage)</returns>
    IQueryBuilder IQueryBuilder.AddCriteria(IDynamoCriteria criteria)
    {
        base.AddCriteria(criteria);
        return this;
    }

    /// <summary>
    /// Sets the Key for the Builder. If the table has a Hash and Sort key, use the method twice, one of each key.
    /// </summary>
    /// <param name="name">The field's name that will be added. Recommended to use `nameof` function to avoid issues when renaming.</param>
    /// <param name="value">The key value</param>
    /// <typeparam name="TK">The type of the key (int, long, string, etc)</typeparam>
    /// <returns>The QueryBuilder (for a fluent API usage)</returns>
    IQueryBuilder IQueryBuilder.SetKey<TK>(string name, TK value)
    {
        base.SetKey(name, value);
        return this;
    }

    /// <summary>
    /// Defines if the Criteria will use parenthesis or not
    /// </summary>
    /// <returns>The QueryBuilder (for a fluent API usage)</returns>
    IQueryBuilder IQueryBuilder.UseParenthesis()
    {
        base.UseParenthesis();
        return this;
    }

    /// <summary>
    /// Sets the Page information.
    /// </summary>
    /// <param name="pageSize">Configures the page size. In case of null, it will use the <see cref="IQueryBuilder.DefaultPageSize"/></param>
    /// <param name="pageToken">Configures the page token, to execute the pagination</param>
    /// <returns>The QueryBuilder (for a fluent API usage)</returns>
    public IQueryBuilder SetPage(int? pageSize, string? pageToken)
        => SetPageSize(pageSize).SetPageToken(pageToken);

    /// <summary>
    /// Sets the Page size information.
    /// </summary>
    /// <param name="pageSize">Configures the page size. In case of null, it will use the <see cref="IQueryBuilder.DefaultPageSize"/></param>
    /// <returns>The QueryBuilder (for a fluent API usage)</returns>
    public IQueryBuilder SetPageSize(int? pageSize)
    {
        _pageSize = pageSize ?? IQueryBuilder.DefaultPageSize;
        return this;
    }
    
    /// <summary>
    /// Sets the Page token information.
    /// </summary>
    /// <param name="pageToken">Configures the page token, to execute the pagination</param>
    /// <returns>The QueryBuilder (for a fluent API usage)</returns>
    public IQueryBuilder SetPageToken(string? pageToken)
    {
        _pageToken = pageToken;
        return this;
    }

    /// <summary>
    /// Sets the Sort/Range key to be used for sorting as descending
    /// </summary>
    /// <returns>The QueryBuilder (for a fluent API usage)</returns>
    public IQueryBuilder OrderByDescending()
    {
        _sortDescending = true;
        return this;
    }

    /// <summary>
    /// Sets the Index name, in case you are using a different index for the query
    /// </summary>
    /// <param name="indexName">The index name to be used</param>
    /// <returns>The QueryBuilder (for a fluent API usage)</returns>
    public IQueryBuilder SetIndexName(string? indexName)
    {
        _indexName = indexName;
        return this;
    }

    private QueryFilter BuildQueryFilter()
    {
        var queryFilter = new QueryFilter();
        foreach (var key in Keys)
            queryFilter.AddCondition(key.Key, QueryOperator.Equal, new List<AttributeValue> { key.Value });

        return queryFilter;
    }

    private Expression BuildExpression() =>
        new()
        {
            ExpressionAttributeNames = Names,
            ExpressionAttributeValues = Entries,
            ExpressionStatement = BuildConditionExpression()
        };

    /// <summary>
    /// Build the DynamoDB object that will be used to do the query operation. Called internally by the
    /// DynaMightContext.
    /// </summary>
    /// <returns>An `QueryOperationConfig` object</returns>
    public QueryOperationConfig Build(bool useFilter)
        => new()
        {
            IndexName = _indexName,
            Filter = BuildQueryFilter(),
            Limit = _pageSize,
            PaginationToken = _pageToken,
            BackwardSearch = _sortDescending,
            FilterExpression = useFilter ? BuildExpression() : null
        };
}
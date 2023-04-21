using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using DynaMight.Criteria;

namespace DynaMight.Builders;

public class QueryBuilder : DynamoBuilder, IQueryBuilder
{
    private int _pageSize = IQueryBuilder.DefaultPageSize;
    private string? _pageToken;
    private bool _sortDescending;
    private string? _indexName;

    private QueryBuilder()
    {
    }

    public static IQueryBuilder Create()
        => new QueryBuilder();

    IQueryBuilder IQueryBuilder.AddCriteria(bool condition, Func<IDynamoCriteria> func)
    {
        base.AddCriteria(condition, func);
        return this;
    }

    IQueryBuilder IQueryBuilder.AddCriteria(IDynamoCriteria criteria)
    {
        base.AddCriteria(criteria);
        return this;
    }

    IQueryBuilder IQueryBuilder.SetKey<K>(string name, K value)
    {
        base.SetKey(name, value);
        return this;
    }

    IQueryBuilder IQueryBuilder.UseParenthesis()
    {
        base.UseParenthesis();
        return this;
    }

    public IQueryBuilder SetPage(int? pageSize, string? pageToken)
        => SetPageSize(pageSize).SetPageToken(pageToken);

    public IQueryBuilder SetPageSize(int? pageSize)
    {
        _pageSize = pageSize ?? IQueryBuilder.DefaultPageSize;
        return this;
    }

    public IQueryBuilder SetPageToken(string? pageToken)
    {
        _pageToken = pageToken;
        return this;
    }

    public IQueryBuilder OrderByDescending()
    {
        _sortDescending = true;
        return this;
    }

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
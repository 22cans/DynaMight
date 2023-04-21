using Amazon.DynamoDBv2.DocumentModel;
using DynaMight.Criteria;

namespace DynaMight.Builders;

public interface IQueryBuilder : IDynamoBuilder
{
    public const int DefaultPageSize = 20;
    IQueryBuilder AddCriteria(bool condition, Func<IDynamoCriteria> func);
    IQueryBuilder AddCriteria(IDynamoCriteria criteria);
    IQueryBuilder SetKey<K>(string name, K value);
    IQueryBuilder SetPage(int? pageSize, string? pageToken);
    IQueryBuilder SetPageSize(int? pageSize);
    IQueryBuilder SetPageToken(string? pageToken);
    IQueryBuilder OrderByDescending();
    IQueryBuilder SetIndexName(string? indexName);
    IQueryBuilder UseParenthesis();
    QueryOperationConfig Build(bool useFilter);
}
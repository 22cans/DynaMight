using Amazon.DynamoDBv2.DocumentModel;
using DynaMight.Criteria;

namespace DynaMight.Builders;

/// <summary>
/// Basic interface for Query Builder
/// </summary>
public interface IQueryBuilder : IDynamoBuilder
{
    /// <summary>
    /// Default size for Pagination
    /// </summary>
    public const int DefaultPageSize = 20;/// <summary>
    /// Adds a new Criteria into the builder
    /// </summary>
    /// <param name="condition">The condition that needs to be satisfied to add the criteria</param>
    /// <param name="func">The criteria that will be added into the builder. It uses a Func, which will be normally a
    /// `() => criteria`.</param>
    /// <returns>The QueryBuilder (for a fluent API usage)</returns>
    IQueryBuilder AddCriteria(bool condition, Func<IDynamoCriteria> func);
    /// <summary>
    /// Adds a new Criteria into the builder
    /// </summary>
    /// <param name="criteria">The criteria that will be added into the builder.</param>
    /// <returns>The QueryBuilder (for a fluent API usage)</returns>
    IQueryBuilder AddCriteria(IDynamoCriteria criteria);
    /// <summary>
    /// Sets the Key for the Builder. If the table has a Hash and Sort key, use the method twice, one of each key.
    /// </summary>
    /// <param name="name">The field's name that will be added. Recommended to use `nameof` function to avoid issues when renaming.</param>
    /// <param name="value">The key value</param>
    /// <typeparam name="TK">The type of the key (int, long, string, etc)</typeparam>
    /// <returns>The QueryBuilder (for a fluent API usage)</returns>
    IQueryBuilder SetKey<TK>(string name, TK value);
    /// <summary>
    /// Sets the Page information.
    /// </summary>
    /// <param name="pageSize">Configures the page size. In case of null, it will use the <see cref="IQueryBuilder.DefaultPageSize"/></param>
    /// <param name="pageToken">Configures the page token, to execute the pagination</param>
    /// <returns>The QueryBuilder (for a fluent API usage)</returns>
    IQueryBuilder SetPage(int? pageSize, string? pageToken);
    /// <summary>
    /// Sets the Page size information.
    /// </summary>
    /// <param name="pageSize">Configures the page size. In case of null, it will use the <see cref="IQueryBuilder.DefaultPageSize"/></param>
    /// <returns>The QueryBuilder (for a fluent API usage)</returns>
    IQueryBuilder SetPageSize(int? pageSize);
    /// <summary>
    /// Sets the Page token information.
    /// </summary>
    /// <param name="pageToken">Configures the page token, to execute the pagination</param>
    /// <returns>The QueryBuilder (for a fluent API usage)</returns>
    IQueryBuilder SetPageToken(string? pageToken);
    /// <summary>
    /// Sets the Sort/Range key to be used for sorting as descending
    /// </summary>
    /// <returns>The QueryBuilder (for a fluent API usage)</returns>
    IQueryBuilder OrderByDescending();
    /// <summary>
    /// Sets the Query to use Strongly Consistent Reads.
    /// </summary>
    /// <remarks>Check https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/HowItWorks.ReadConsistency.html for more information</remarks>
    /// <returns>The QueryBuilder (for a fluent API usage)</returns>
    IQueryBuilder ConsistentRead();
    /// <summary>
    /// Sets the Index name, in case you are using a different index for the query
    /// </summary>
    /// <param name="indexName">The index name to be used</param>
    /// <returns>The QueryBuilder (for a fluent API usage)</returns>
    IQueryBuilder SetIndexName(string? indexName);
    /// <summary>
    /// Defines if the Criteria will use parenthesis or not
    /// </summary>
    /// <returns>The QueryBuilder (for a fluent API usage)</returns>
    IQueryBuilder UseParenthesis();
    /// <summary>
    /// Build the DynamoDB object that will be used to do the query operation. Called internally by the
    /// DynaMightContext.
    /// </summary>
    /// <returns>An `QueryOperationConfig` object</returns>
    QueryOperationConfig Build(bool useFilter);
}
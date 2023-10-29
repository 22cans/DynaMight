using Amazon.DynamoDBv2.DataModel;
using DynaMight.BatchWrapper;
using DynaMight.Builders;
using DynaMight.Pagination;

namespace DynaMight;

/// <summary>
/// Context interface for using the DataModel mode of DynamoDB.
/// Used to interact with the service, save/load objects, etc.
/// </summary>
public interface IDynaMightContext : IDynamoDBContext
{
    /// <summary>
    /// Get all the items based on the QueryBuilder.
    /// </summary>
    /// <param name="queryBuilder">The QueryBuilder for the query</param>
    /// <param name="cancellationToken">Token which can be used to cancel the task.</param>
    /// <typeparam name="T">C# object type</typeparam>
    /// <returns>A Task of List of T that can be used to poll or wait for results, or both.</returns>
    /// <remarks>
    /// Internally, this method uses the <see cref="AsyncSearch{T}.GetRemainingAsync"/>, which will get all items from the
    /// database that evaluates for the query filters. Use with responsibility. 
    /// </remarks>
    Task<List<T>> GetAll<T>(IQueryBuilder queryBuilder, CancellationToken cancellationToken);
    /// <summary>
    /// Gets a page of items based on the QueryBuilder. Criteria are not used for this method.
    /// </summary>
    /// <param name="queryBuilder">The QueryBuilder for the query</param>
    /// <param name="cancellationToken">Token which can be used to cancel the task.</param>
    /// <typeparam name="T">C# object type</typeparam>
    /// <returns>A Task of Page of T that can be used to poll or wait for results, or both.</returns>
    Task<Page<T>> GetPage<T>(IQueryBuilder queryBuilder, CancellationToken cancellationToken);
    /// <summary>
    /// Gets a filtered page of items based on the QueryBuilder. Criteria are used in this method.
    /// </summary>
    /// <param name="queryBuilder">The QueryBuilder for the query</param>
    /// <param name="cancellationToken">Token which can be used to cancel the task.</param>
    /// <typeparam name="T">C# object type</typeparam>
    /// <returns>A Task of Page of T that can be used to poll or wait for results, or both.</returns>
    /// <remarks>
    /// As DynamoDB does not does the filter in the database level, this method will do a while loop to get until it gets
    /// the required number of items defined in the QueryBuilder or reaches the end of the table. Use with responsibility. 
    /// </remarks>
    Task<Page<T>> GetFilteredPage<T>(IQueryBuilder queryBuilder, CancellationToken cancellationToken);

    /// <summary>
    /// Executes an AtomicOperation defined by the AtomicBuilder
    /// </summary>
    /// <param name="requestBuilder">The AtomicBuilder which defines the operations/criteria</param>
    /// <param name="cancellationToken">Token which can be used to cancel the task.</param>
    /// <typeparam name="T">C# object type</typeparam>
    /// <returns>The class with the updated values from DynamoDB</returns>
    Task<T> ExecuteAtomicOperation<T>(IAtomicBuilder requestBuilder, CancellationToken cancellationToken)
        where T : new();

    /// <summary>
    /// Creates a BatchGet with DynamoDB's default implementation
    /// </summary>
    /// <typeparam name="T">C# object type</typeparam>
    /// <returns>An <see cref="IBatchGet{T}"/> implementation</returns>
    IBatchGet<T> CreateBatchGet<T>();
    /// <summary>
    /// Creates a BatchGet with DynamoDB's default implementation
    /// </summary>
    /// <param name="config">The DynamoDBOperationConfig for the batch.</param>
    /// <typeparam name="T">C# object type</typeparam>
    /// <returns>An <see cref="IBatchGet{T}"/> implementation</returns>
    new IBatchGet<T> CreateBatchGet<T>(DynamoDBOperationConfig config);
    /// <summary>
    /// Creates a BatchWrite with DynamoDB's default implementation
    /// </summary>
    /// <typeparam name="T">C# object type</typeparam>
    /// <returns>An <see cref="IBatchWrite{T}"/> implementation</returns>
    IBatchWrite<T> CreateBatchWrite<T>();
    /// <summary>
    /// Creates a BatchWrite with DynamoDB's default implementation
    /// </summary>
    /// <param name="config">The DynamoDBOperationConfig for the batch.</param>
    /// <typeparam name="T">C# object type</typeparam>
    /// <returns>An <see cref="IBatchWrite{T}"/> implementation</returns>
    new IBatchWrite<T> CreateBatchWrite<T>(DynamoDBOperationConfig config);
    /// <summary>
    /// Creates a MultiTable BatchGet with DynamoDB's default implementation
    /// </summary>
    /// <param name="batches">The batches that will be added to execution</param>
    /// <returns>An <see cref="IMultiTableBatchGet"/> implementation</returns>
    IMultiTableBatchGet CreateMultiTableBatchGet(params IBatchGet[] batches);
    /// <summary>
    /// Creates a MultiTable BatchWrite with DynamoDB's default implementation
    /// </summary>
    /// <param name="batches">The batches that will be added to execution</param>
    /// <returns>An <see cref="IMultiTableBatchWrite"/> implementation</returns>
    IMultiTableBatchWrite CreateMultiTableBatchWrite(params IBatchWrite[] batches);
}
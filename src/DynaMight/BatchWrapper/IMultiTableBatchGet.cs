namespace DynaMight.BatchWrapper;

/// <summary>
/// Interface for retrieving a batch of items from multiple DynamoDB tables,
/// using multiple strongly-typed BatchGet objects
/// </summary>
public interface IMultiTableBatchGet
{
    /// <summary>
    /// Add a BatchGet object to the multi-table batch request
    /// </summary>
    /// <param name="batch">BatchGet to add</param>
    void AddBatch(IBatchGet batch);
    /// <summary>
    /// Initiates the asynchronous execution of the Execute operation.
    /// <seealso cref="!:Amazon.DynamoDBv2.DataModel.MultiTableBatchGet.Execute" />
    /// </summary>
    /// <param name="cancellationToken">Token which can be used to cancel the task.</param>
    /// <returns>A Task that can be used to poll or wait for results, or both.</returns>
    Task ExecuteAsync(CancellationToken cancellationToken);
}
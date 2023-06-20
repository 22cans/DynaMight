namespace DynaMight.BatchWrapper;

/// <summary>
/// Interface for writing/deleting a batch of items in multiple DynamoDB tables,
/// using multiple strongly-typed BatchWrite objects
/// </summary>
public interface IMultiTableBatchWrite
{
    /// <summary>
    /// Add a BatchWrite object to the multi-table batch request
    /// </summary>
    /// <param name="batch">BatchGet to add</param>
    void AddBatch(IBatchWrite batch);
    /// <summary>
    /// Initiates the asynchronous execution of the Execute operation.
    /// <seealso cref="!:Amazon.DynamoDBv2.DataModel.MultiTableBatchWrite.Execute" />
    /// </summary>
    /// <param name="cancellationToken">Token which can be used to cancel the task.</param>
    /// <returns>A Task that can be used to poll or wait for results, or both.</returns>
    Task ExecuteAsync(CancellationToken cancellationToken);
}
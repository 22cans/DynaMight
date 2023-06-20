namespace DynaMight.BatchWrapper;

/// <summary>
/// Represents a non-generic object for writing/deleting a batch of items
/// in a single DynamoDB table
/// </summary>
public interface IBatchWrite
{
}


/// <summary>
/// Represents a generic object for writing/deleting a batch of items
/// in a single DynamoDB table
/// </summary>
public interface IBatchWrite<in T> : IBatchWrite
{
    /// <summary>
    /// Add a single item to be put in the current batch operation
    /// </summary>
    /// <param name="item"></param>
    void AddPutItem(T item);
    /// <summary>
    /// Add a number of items to be put in the current batch operation
    /// </summary>
    /// <param name="items">Items to put</param>
    void AddPutItems(IEnumerable<T> items);/// <summary>
    /// Add a single item to be deleted in the current batch operation.
    /// Item is identified by its hash-and-sort primary key.
    /// </summary>
    /// <param name="hashKey">Hash key of the item to delete</param>
    /// <param name="sortKey">Range key of the item to delete</param>
    void AddDeleteKey(object hashKey, object? sortKey = null);
    /// <summary>
    /// Add a single item to be deleted in the current batch operation.
    /// </summary>
    /// <param name="item">Item to be deleted</param>
    void AddDeleteItem(T item);
    /// <summary>
    /// Add a number of items to be deleted in the current batch operation
    /// </summary>
    /// <param name="items">Items to be deleted</param>
    void AddDeleteItems(IEnumerable<T> items);
    /// <summary>
    /// Initiates the asynchronous execution of the Execute operation.
    /// <seealso cref="!:Amazon.DynamoDBv2.DataModel.BatchWrite.Execute" />
    /// </summary>
    /// <param name="cancellationToken">Token which can be used to cancel the task.</param>
    /// <returns>A Task that can be used to poll or wait for results, or both.</returns>
    Task ExecuteAsync(CancellationToken cancellationToken);
}
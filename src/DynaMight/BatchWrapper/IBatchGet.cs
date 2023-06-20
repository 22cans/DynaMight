namespace DynaMight.BatchWrapper;

/// <summary>
/// Represents a non-generic object for retrieving a batch of items from a single DynamoDB table
/// </summary>
public interface IBatchGet { }

/// <summary>
/// Represents a generic object for retrieving a batch of items from a single DynamoDB table
/// </summary>
public interface IBatchGet<T> : IBatchGet
{
    /// <summary>Add a single item to get.</summary>
    /// <param name="keyObject">Object key of the item to get</param>
    void AddKey(T keyObject);
    /// <summary>
    /// Add a single item to get, identified by its hash primary key.
    /// </summary>
    /// <param name="hashKey">Hash key of the item to get</param>
    void AddKey(object hashKey);
    /// <summary>
    /// Add a single item to get, identified by its hash-and-range primary key.
    /// </summary>
    /// <param name="hashKey">Hash key of the item to get</param>
    /// <param name="rangeKey">Range key of the item to get</param>
    void AddKey(object hashKey, object rangeKey);
    /// <summary>
    /// Initiates the asynchronous execution of the Execute operation.
    /// <seealso cref="!:Amazon.DynamoDBv2.DataModel.BatchGet.Execute" />
    /// </summary>
    /// <param name="cancellationToken">Token which can be used to cancel the task.</param>
    /// <returns>A Task that can be used to poll or wait for results, or both.</returns>
    Task ExecuteAsync(CancellationToken cancellationToken);
    /// <summary>
    /// List of results retrieved from DynamoDB.
    /// Populated after Execute is called.
    /// </summary>
    List<T> Results { get; }
}
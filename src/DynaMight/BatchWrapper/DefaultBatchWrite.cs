using System.Diagnostics.CodeAnalysis;
using Amazon.DynamoDBv2.DataModel;

namespace DynaMight.BatchWrapper;

/// <summary>
/// Represents a non-generic object for writing/deleting a batch of items in a single DynamoDB table
/// </summary>
[ExcludeFromCodeCoverage]
public class DefaultBatchWrite
{
    private readonly BatchWrite _batchWrite;
    
    /// <summary>
    /// Creates the BatchWrite with the DynamoDB default implementation
    /// </summary>
    /// <param name="batchWrite">DynamoDB BatchWrite object</param>
    protected DefaultBatchWrite(BatchWrite batchWrite)
    {
        _batchWrite = batchWrite;
    }

    /// <summary>
    /// Gets the DynamoDB BatchGet object
    /// </summary>
    /// <returns>DynamoDB BatchGet</returns>
    public BatchWrite GetBatchWrite() => _batchWrite;
}

/// <inheritdoc cref="DynaMight.BatchWrapper.IBatchWrite{T}" />
[ExcludeFromCodeCoverage]
public class DefaultBatchWrite<T> : DefaultBatchWrite, IBatchWrite<T>
{
    private readonly BatchWrite<T> _batchWrite;

    /// <inheritdoc />
    public DefaultBatchWrite(BatchWrite<T> batchWrite) : base(batchWrite)
    {
        _batchWrite = batchWrite;
    }

    /// <inheritdoc />
    public void AddPutItem(T item) => _batchWrite.AddPutItem(item);

    /// <inheritdoc />
    public void AddPutItems(IEnumerable<T> items) => _batchWrite.AddPutItems(items);

    /// <inheritdoc />
    public void AddDeleteKey(object hashKey, object? sortKey = null) => _batchWrite.AddDeleteKey(hashKey, sortKey);

    /// <inheritdoc />
    public void AddDeleteItem(T item) => _batchWrite.AddDeleteItem(item);

    /// <inheritdoc />
    public void AddDeleteItems(IEnumerable<T> items) => _batchWrite.AddDeleteItems(items);

    /// <inheritdoc />
    public Task ExecuteAsync(CancellationToken cancellationToken) =>
        _batchWrite.ExecuteAsync(cancellationToken);
}
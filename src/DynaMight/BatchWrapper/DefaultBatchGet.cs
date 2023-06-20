using System.Diagnostics.CodeAnalysis;
using Amazon.DynamoDBv2.DataModel;

namespace DynaMight.BatchWrapper;

/// <summary>
/// Represents a non-generic object for retrieving a batch of items from a single DynamoDB table wrapper
/// </summary>
[ExcludeFromCodeCoverage]
public abstract class DefaultBatchGet
{
    private readonly BatchGet _batchGet;

    /// <summary>
    /// Creates the BatchGet with the DynamoDB default implementation
    /// </summary>
    /// <param name="batchGet">DynamoDB BatchGet object</param>
    protected DefaultBatchGet(BatchGet batchGet) => _batchGet = batchGet;

    /// <summary>
    /// Gets the DynamoDB BatchGet object
    /// </summary>
    /// <returns>DynamoDB BatchGet</returns>
    public BatchGet GetBatchGet() => _batchGet;
}

/// <inheritdoc cref="DynaMight.BatchWrapper.IBatchGet{T}" />
[ExcludeFromCodeCoverage]
public class DefaultBatchGet<T> : DefaultBatchGet, IBatchGet<T>
{
    private readonly BatchGet<T> _batchGet;

    /// <inheritdoc />
    public List<T> Results => _batchGet.Results;

    /// <inheritdoc />
    public DefaultBatchGet(BatchGet<T> batchGet) : base(batchGet) => _batchGet = batchGet;

    /// <inheritdoc />
    public void AddKey(T item) => _batchGet.AddKey(item);

    /// <inheritdoc />
    public void AddKey(object hashKey) => _batchGet.AddKey(hashKey);

    /// <inheritdoc />
    public void AddKey(object hashKey, object rangeKey) => _batchGet.AddKey(hashKey, rangeKey);

    /// <inheritdoc />
    public Task ExecuteAsync(CancellationToken cancellationToken) => _batchGet.ExecuteAsync(cancellationToken);
}
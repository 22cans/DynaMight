using System.Diagnostics.CodeAnalysis;
using Amazon.DynamoDBv2.DataModel;

namespace DynaMight.BatchWrapper;

/// <summary>
/// Class for retrieving a batch of items from multiple DynamoDB tables, using multiple strongly-typed BatchGet objects wrapper
/// </summary>
[ExcludeFromCodeCoverage]
public class DefaultMultiTableBatchGet : IMultiTableBatchGet
{
    private readonly MultiTableBatchGet _multiTableBatchGet;

    /// <summary>
    /// Constructs a MultiTableBatchGet object from a number of
    /// BatchGet objects
    /// </summary>
    /// <param name="multiTableBatchGet">DynamoDB original MultiTableBatchGet</param>
    /// <param name="batches">Collection of BatchGet objects</param>
    public DefaultMultiTableBatchGet(MultiTableBatchGet multiTableBatchGet, params IBatchGet[] batches)
    {
        _multiTableBatchGet = multiTableBatchGet;
        foreach (var batch in batches)
            _multiTableBatchGet.AddBatch((batch as DefaultBatchGet)?.GetBatchGet());
    }

    /// <inheritdoc />
    public void AddBatch(IBatchGet batch)
        => _multiTableBatchGet.AddBatch((batch as DefaultBatchGet)?.GetBatchGet());

    /// <inheritdoc />
    public Task ExecuteAsync(CancellationToken cancellationToken) =>
        _multiTableBatchGet.ExecuteAsync(cancellationToken);
}
using System.Diagnostics.CodeAnalysis;
using Amazon.DynamoDBv2.DataModel;

namespace DynaMight.BatchWrapper;

/// <summary>
/// Class for writing/deleting a batch of items in multiple DynamoDB tables, using multiple strongly-typed BatchWrite objects wrapper
/// </summary>
[ExcludeFromCodeCoverage]
public class DefaultMultiTableBatchWrite : IMultiTableBatchWrite
{
    private readonly MultiTableBatchWrite _multiTableBatchWrite;

    /// <summary>
    /// Constructs a MultiTableBatchWrite object from a number of
    /// BatchWrite objects
    /// </summary>
    /// <param name="multiTableBatchWrite">DynamoDB original MultiTableBatchWrite</param>
    /// <param name="batches">Collection of BatchWrite objects</param>
    public DefaultMultiTableBatchWrite(MultiTableBatchWrite multiTableBatchWrite, params IBatchWrite[] batches)
    {
        _multiTableBatchWrite = multiTableBatchWrite;
        foreach (var batch in batches)
            _multiTableBatchWrite.AddBatch((batch as DefaultBatchWrite)?.GetBatchWrite());
    }

    /// <inheritdoc />
    public void AddBatch(IBatchWrite batch)
        => _multiTableBatchWrite.AddBatch((batch as DefaultBatchWrite)?.GetBatchWrite());

    /// <inheritdoc />
    public Task ExecuteAsync(CancellationToken cancellationToken) =>
        _multiTableBatchWrite.ExecuteAsync(cancellationToken);
}
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

    public DefaultMultiTableBatchGet(MultiTableBatchGet multiTableBatchGet, params IBatchGet[] batches)
    {
        _multiTableBatchGet = multiTableBatchGet;
        foreach (var batch in batches)
            _multiTableBatchGet.AddBatch((batch as DefaultBatchGet)?.GetBatchGet());
    }

    public void AddBatch(IBatchGet batch)
        => _multiTableBatchGet.AddBatch((batch as DefaultBatchGet)?.GetBatchGet());

    public Task ExecuteAsync(CancellationToken cancellationToken) =>
        _multiTableBatchGet.ExecuteAsync(cancellationToken);
}
using Amazon.DynamoDBv2.DataModel;

namespace DynaMight.BatchWrapper;

public class DefaultMultiTableBatchWrite : IMultiTableBatchWrite
{
    private readonly MultiTableBatchWrite _multiTableBatchWrite;

    public DefaultMultiTableBatchWrite(MultiTableBatchWrite multiTableBatchWrite, params IBatchWrite[] batches)
    {
        _multiTableBatchWrite = multiTableBatchWrite;
        foreach (var batch in batches)
            _multiTableBatchWrite.AddBatch((batch as DefaultBatchWrite)?.GetBatchWrite());
    }

    public void AddBatch(IBatchWrite batch)
        => _multiTableBatchWrite.AddBatch((batch as DefaultBatchWrite)?.GetBatchWrite());

    public Task ExecuteAsync(CancellationToken cancellationToken) =>
        _multiTableBatchWrite.ExecuteAsync(cancellationToken);
}
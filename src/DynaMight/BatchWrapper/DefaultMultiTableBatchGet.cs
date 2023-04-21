using Amazon.DynamoDBv2.DataModel;

namespace DynaMight.BatchWrapper;

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
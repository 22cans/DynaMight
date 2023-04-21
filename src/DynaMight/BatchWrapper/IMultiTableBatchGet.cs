namespace DynaMight.BatchWrapper;

public interface IMultiTableBatchGet
{
    void AddBatch(IBatchGet batch);
    Task ExecuteAsync(CancellationToken cancellationToken);
}
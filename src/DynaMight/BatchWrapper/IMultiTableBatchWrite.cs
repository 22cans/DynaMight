namespace DynaMight.BatchWrapper;

public interface IMultiTableBatchWrite
{
    void AddBatch(IBatchWrite batch);
    Task ExecuteAsync(CancellationToken cancellationToken);
}
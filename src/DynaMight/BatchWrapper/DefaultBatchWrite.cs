using Amazon.DynamoDBv2.DataModel;

namespace DynaMight.BatchWrapper;

public class DefaultBatchWrite
{
    private readonly BatchWrite _batchWrite;

    protected DefaultBatchWrite(BatchWrite batchWrite)
    {
        _batchWrite = batchWrite;
    }

    public BatchWrite GetBatchWrite() => _batchWrite;
}

public class DefaultBatchWrite<T> : DefaultBatchWrite, IBatchWrite<T>
{
    private readonly BatchWrite<T> _batchWrite;

    public DefaultBatchWrite(BatchWrite<T> batchWrite) : base(batchWrite)
    {
        _batchWrite = batchWrite;
    }

    public void AddPutItem(T item) => _batchWrite.AddPutItem(item);
    public void AddPutItems(IEnumerable<T> items) => _batchWrite.AddPutItems(items);
    public void AddDeleteKey(object hashKey, object? sortKey = null) => _batchWrite.AddDeleteKey(hashKey, sortKey);
    public void AddDeleteItem(T item) => _batchWrite.AddDeleteItem(item);
    public void AddDeleteItems(IEnumerable<T> items) => _batchWrite.AddDeleteItems(items);

    public Task ExecuteAsync(CancellationToken cancellationToken) =>
        _batchWrite.ExecuteAsync(cancellationToken);
}
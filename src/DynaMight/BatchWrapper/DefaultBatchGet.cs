using Amazon.DynamoDBv2.DataModel;

namespace DynaMight.BatchWrapper;

public class DefaultBatchGet
{
    private readonly BatchGet _batchGet;

    protected DefaultBatchGet(BatchGet batchGet) => _batchGet = batchGet;

    public BatchGet GetBatchGet() => _batchGet;
}

public class DefaultBatchGet<T> : DefaultBatchGet, IBatchGet<T>
{
    private readonly BatchGet<T> _batchGet;
    public List<T> Results => _batchGet.Results;

    public DefaultBatchGet(BatchGet<T> batchGet) : base(batchGet) => _batchGet = batchGet;

    public void AddKey(T item) => _batchGet.AddKey(item);
    public void AddKey(object hashKey) => _batchGet.AddKey(hashKey);
    public void AddKey(object hashKey, object rangeKey) => _batchGet.AddKey(hashKey, rangeKey);
    public Task ExecuteAsync(CancellationToken cancellationToken) => _batchGet.ExecuteAsync(cancellationToken);
}
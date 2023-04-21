namespace DynaMight.BatchWrapper;

public interface IBatchGet { }

public interface IBatchGet<T> : IBatchGet
{
    void AddKey(T item);
    void AddKey(object hashKey);
    void AddKey(object hashKey, object rangeKey);
    Task ExecuteAsync(CancellationToken cancellationToken);
    List<T> Results { get; }
}
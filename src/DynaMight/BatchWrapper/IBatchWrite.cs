namespace DynaMight.BatchWrapper;

public interface IBatchWrite
{
}

public interface IBatchWrite<in T> : IBatchWrite
{
    void AddPutItem(T item);
    void AddPutItems(IEnumerable<T> items);
    void AddDeleteKey(object hashKey, object? sortKey = null);
    void AddDeleteItem(T item);
    void AddDeleteItems(IEnumerable<T> items);
    Task ExecuteAsync(CancellationToken cancellationToken);
}
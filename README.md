# DynaMight

DynaMight is a powerful library wrapper for Amazon's DynamoDB SDK, designed to simplify the usage of complex features. It provides a convenient way to execute atomic operations, implement pagination, and filter results.
​
## Features
​
- **Easier operations using builder:** the DynaMight uses the builder pattern to make it easier to create the configurations for queries and atomic operations.
- **More readable code:** the DynaMight uses a fluent API pattern, which makes the code more readable.
- Effortless pagination and filtering: Performing pagination and filtering in the DynamoDB SDK can be challenging. DynaMight abstracts away this complexity.

## Examples

Consider the following classes below:

```cs
[DynamoDBTable("User")]
public class User
{
    [DynamoDBHashKey]
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
}

[DynamoDBTable("User")]
public class Session
{
    [DynamoDBHashKey]
    public string UserId { get; set; } = default!;
    [DynamoDBRangeKey]
    public string SessionId { get; set; } = default!;
    public SessionStatus Status { get; set; } = default!;
    public int Clicks { get; set; }
}

public enum SessionStatus
{
    Active,
    Inactive
}
```

To instruct DynaMight on how to handle the `SessionStatus` enum, you can use the `DynaMightCustomConverter` attribute, which registers the default converter (enum to int transformation) when the `DynaMightCustomConverter.LoadAndRegister()` function is called.

```cs
[DynaMightCustomConverter]
public enum SessionStatus
{
    Active,
    Inactive
}
```

### Registering DynaMight

```cs
var client = new AmazonDynamoDBClient();
var config = new DynamoDBContextConfig();
var dbContext = new DynaMightDbContext(client, config);
services.AddSingleton<IDynamoDBContext>(dbContext);
```
If you need to provide credentials to create the `AmazonDynamoDBClient`, please do so as usual.

We recommend calling the `LoadAndRegister` function during registration, which loads all classes and enums marked with the `DynaMightCustomConverter` attribute and registers them.

```cs
DynaMightCustomConverter.LoadAndRegister();
```

### Retrieving all sessions for a user

```cs
public async Task<List<Session>> GetAllSessions(CancellationToken cancellationToken)
{
    var config = QueryBuilder
        .Create()
        .SetKey(nameof(Session.UserId), "userId1");

    return await _dynaMightContext.GetAll<Session>(config, cancellationToken);
}
```

### Retrieving paginated sessions for a user

```cs
public async Task<Page<Session>> GetSessionPaginated(int? pageSize, string? pageToken, CancellationToken cancellationToken)
{
    var config = QueryBuilder
        .Create()
        .SetKey(nameof(Session.UserId), "userId1")
        .SetPage(pageSize, pageToken);

    return await _dynaMightContext.GetPage<Session>(config, cancellationToken);
}
```

The `Page<T>` class has two properties: `string PageToken` and `IList<T>? Results`. The `PageToken` should be passed back to retrieve the next page.

### Retrieving paginated sessions for a user with filtering for inactive sessions

```cs
public async Task<List<Session>> GetInactiveSessionsPaginated(int? pageSize, string? pageToken, CancellationToken cancellationToken)
{
    var config = QueryBuilder
        .Create()
        .SetKey(nameof(Session.UserId), "userId1")
        .SetPage(pageSize, pageToken)
        .AddCriteria(new EqualDynamoCriteria<SessionStatus>(nameof(Session.Status), SessionStatus.Inactive));

    return await _dynaMightContext.GetFilteredPage<Session>(config, cancellationToken);
}
```
### Retrieving paginated sessions for a user with filtering for inactive sessions and sessions with 100 clicks or more

```cs
public async Task<Page<Session>> GetInactiveSessionsOver100ClicksPaginated(int? pageSize, string? pageToken, CancellationToken cancellationToken)
{
    var config = QueryBuilder
        .Create()
        .SetKey(nameof(Session.UserId), "userId1")
        .SetPage(pageSize, pageToken)
        .AddCriteria(new AndDynamoCriteria(
            new EqualDynamoCriteria<SessionStatus>(nameof(Session.Status), SessionStatus.Inactive),
            new GreaterOrEqualDynamoCriteria<int>(nameof(Session.Clicks), 100)));

    return await _dynaMightContext.GetFilteredPage<Session>(config, cancellationToken);
}
```

### Adding a new click in an atomic operation

```cs
public async Task<Session> AddClick(CancellationToken cancellationToken)
{
    var config = AtomicBuilder
        .Create()
        .SetKey(nameof(Session.UserId), "userId1")
        .AddOperation(new IncrementByAtomicOperation<int>(nameof(Session.Clicks), 1));

    return await _dynaMightContext.ExecuteAtomicOperation<Session>(config, cancellationToken);
}
```

### Creating a read batch

```cs
public async Task<List<Session>> BatchRead(CancellationToken cancellationToken)
{
    var batch = _dynaMightContext.CreateBatchGet<Session>();
    batch.AddKey("userId1");
    batch.AddKey("userId2");
    
    await batch.ExecuteAsync(cancellationToken);

    return batch.Results;
}
```
​
## License
​
This project is licensed under the [BSD 3-Clause License](LICENSE).
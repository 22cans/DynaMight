# DynaMight

The DynaMight is a library wrapper for the DynamoDB SDK from Amazon, which implements some features that are quite complicated to use directly. It provides a convenient way to execute atomic operations, pagination and filter results.
​
## Features
​
- **Easier operations using builder and fluent API pattern:** the DynaMight tries to facilitate operations using a fluent API pattern and make the operations easier using the builder pattern. See below some examples.
- **Easier pagination and filter queries:** Pagination and filters in DynamoDB SDK is not an easy task. DynaMight wrapper the complexity.

## Examples

Consider for the following events the classes below:

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

We need to tell the DynaMight how to handle this enum. To do that, we can use the `DynaMightCustomConverter` attribute, which will register the enum default converter (enum to int transformation) when the `DynaMightCustomConverter.LoadAndRegister()` function is called.

```cs
[DynaMightCustomConverter]
public enum SessionStatus
{
    Active,
    Inactive
}
```

### How to register the DynaMight

```cs
var client = new AmazonDynamoDBClient();
var config = new DynamoDBContextConfig();
var dbContext = new DynaMightDbContext(client, config);
services.AddSingleton<IDynamoDBContext>(dbContext);
```
If you need to provide the credentials to create the `AmazonDynamoDBClient`, please do that as normal.

We recommend that you call the `LoadAndRegister` function during the registration, which will load all the classes and enums that have the `DynaMightCustomConverter` and register them.

```cs
DynaMightCustomConverter.LoadAndRegister();
```

### How to get all sessions from an user

```cs
public async Task<List<Session>> GetAllSessions(CancellationToken cancellationToken)
{
    var config = QueryBuilder
        .Create()
        .SetKey(nameof(Session.UserId), "userId1");

    return await _dynaMightContext.GetAll<Session>(config, cancellationToken);
}
```

### How to get all sessions from an user using pagination

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

The `Page<T>` class has two properties: `string PageToken` and `IList<T>? Results`. The `PageToken` should be passed back to the function when you want to get the next page.

### How to get all sessions from an user using pagination and filtering to get only the Inactive

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
### How to get all sessions from an user using pagination and filtering to get only the Inactive AND the ones that have 100 clicks or more

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

### How to add a new click in an atomic operation

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

### How to create a read batch

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
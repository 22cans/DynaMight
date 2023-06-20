using Amazon.DynamoDBv2.DataModel;
using DynaMight.AtomicOperations;
using DynaMight.Builders;
using DynaMight.Converters;
using DynaMight.Criteria;
using DynaMight.Pagination;

namespace DynaMight.UnitTests;

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

[DynaMightCustomConverter]
public enum SessionStatus
{
    Active,
    Inactive
}

public class Examples
{
    private readonly IDynaMightContext _dynaMightContext;

    public Examples(IDynaMightContext dynaMightContext)
    {
        _dynaMightContext = dynaMightContext;
    }

    public async Task<List<Session>> GetAllSessions(CancellationToken cancellationToken)
    {
        var config = QueryBuilder
            .Create()
            .SetKey(nameof(Session.UserId), "userId1");

        return await _dynaMightContext.GetAll<Session>(config, cancellationToken);
    }
    
    public async Task<Page<Session>> GetSessionPaginated(int? pageSize, string? pageToken, CancellationToken cancellationToken)
    {
        var config = QueryBuilder
            .Create()
            .SetKey(nameof(Session.UserId), "userId1")
            .SetPage(pageSize, pageToken);

        return await _dynaMightContext.GetPage<Session>(config, cancellationToken);
    }
    
    public async Task<Page<Session>> GetInactiveSessionsPaginated(int? pageSize, string? pageToken, CancellationToken cancellationToken)
    {
        var config = QueryBuilder
            .Create()
            .SetKey(nameof(Session.UserId), "userId1")
            .SetPage(pageSize, pageToken)
            .AddCriteria(new EqualDynamoCriteria<SessionStatus>(nameof(Session.Status), SessionStatus.Inactive));

        return await _dynaMightContext.GetFilteredPage<Session>(config, cancellationToken);
    }
    
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
    
    public async Task<Session> AddClick(CancellationToken cancellationToken)
    {
        var config = AtomicBuilder
            .Create()
            .SetKey(nameof(Session.UserId), "userId1")
            .AddOperation(new IncrementByAtomicOperation<int>(nameof(Session.Clicks), 1));

        return await _dynaMightContext.ExecuteAtomicOperation<Session>(config, cancellationToken);
    }

    public async Task<List<Session>> BatchRead(CancellationToken cancellationToken)
    {
        var batch = _dynaMightContext.CreateBatchGet<Session>();
        batch.AddKey("userId1");
        batch.AddKey("userId2");
        
        await batch.ExecuteAsync(cancellationToken);

        return batch.Results;
    }
}
using Amazon.DynamoDBv2.DataModel;
using DynaMight.AtomicOperations;
using DynaMight.BatchWrapper;
using DynaMight.Builders;
using DynaMight.Pagination;

namespace DynaMight;

public interface IDynaMightContext : IDynamoDBContext
{
    Task<List<T>> GetAll<T>(IQueryBuilder queryBuilder, CancellationToken cancellationToken);
    Task<Page<T>> GetPage<T>(IQueryBuilder queryBuilder, CancellationToken cancellationToken);
    Task<Page<T>> GetFilteredPage<T>(IQueryBuilder queryBuilder, CancellationToken cancellationToken);

    Task<T> ExecuteAtomicOperation<T>(IAtomicBuilder requestBuilder, CancellationToken cancellationToken)
        where T : IAtomicConverter<T>;

    IBatchGet<T> CreateBatchGet<T>();
    IBatchWrite<T> CreateBatchWrite<T>();
    IMultiTableBatchGet CreateMultiTableBatchGet(params IBatchGet[] batches);
    IMultiTableBatchWrite CreateMultiTableBatchWrite(params IBatchWrite[] batches);
}
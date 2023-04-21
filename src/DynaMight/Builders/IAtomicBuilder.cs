using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using DynaMight.AtomicOperations;
using DynaMight.Criteria;

namespace DynaMight.Builders;

public interface IAtomicBuilder : IDynamoBuilder
{
    IAtomicBuilder AddOperation(bool condition, Func<IAtomicOperation> func);
    IAtomicBuilder AddOperation(IAtomicOperation atomicOperation);
    IAtomicBuilder SetKey<K>(string name, K value);
    IAtomicBuilder AddCriteria(bool condition, Func<IDynamoCriteria> func);
    IAtomicBuilder AddCriteria(IDynamoCriteria criteria);
    UpdateItemRequest Build();
    IAtomicBuilder UseParenthesis();
    IAtomicBuilder SetTableName<T>(DynamoDBContextConfig dynamoConfig);
}
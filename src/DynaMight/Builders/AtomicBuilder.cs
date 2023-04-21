using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using DynaMight.AtomicOperations;
using DynaMight.Criteria;

namespace DynaMight.Builders;

public class AtomicBuilder : DynamoBuilder, IAtomicBuilder
{
    private readonly Dictionary<string, IList<IAtomicOperation>> _atomicOperations = new();

    private AtomicBuilder()
    {
    }

    public static IAtomicBuilder Create() => new AtomicBuilder();

    public IAtomicBuilder AddOperation(bool condition, Func<IAtomicOperation> func)
        => condition ? AddOperation(func()) : this;

    public IAtomicBuilder AddOperation(IAtomicOperation atomicOperation)
    {
        if (!_atomicOperations.ContainsKey(atomicOperation.UpdateExpressionType))
        {
            _atomicOperations.Add(atomicOperation.UpdateExpressionType, new List<IAtomicOperation>());
        }

        _atomicOperations[atomicOperation.UpdateExpressionType].Add(atomicOperation);
        AddNameExpression(atomicOperation.GetNameExpression());
        AddValueExpression(atomicOperation.GetValueExpression());

        return this;
    }

    IAtomicBuilder IAtomicBuilder.SetKey<K>(string name, K value)
    {
        base.SetKey(name, value);
        return this;
    }
    IAtomicBuilder IAtomicBuilder.AddCriteria(bool condition, Func<IDynamoCriteria> func)
    {
        base.AddCriteria(condition, func);
        return this;
    }

    IAtomicBuilder IAtomicBuilder.AddCriteria(IDynamoCriteria criteria)
    {
        base.AddCriteria(criteria);
        return this;
    }


    IAtomicBuilder IAtomicBuilder.UseParenthesis()
    {
        base.UseParenthesis();
        return this;
    }

    IAtomicBuilder IAtomicBuilder.SetTableName<T>(DynamoDBContextConfig dynamoConfig)
    {
        base.SetTableName<T>(dynamoConfig);
        return this;
    }

    private string BuildUpdateExpression()
    {
        var updateExpression = new List<string>();
        foreach (var updateExpressionType in _atomicOperations.Keys)
        {
            var operations = string.Join(',',
                _atomicOperations[updateExpressionType].Select(x => x.GetUpdateExpression()));
            updateExpression.Add($"{updateExpressionType} {operations}");
        }

        return string.Join(' ', updateExpression);
    }

    public UpdateItemRequest Build()
    {
        return new UpdateItemRequest
        {
            Key = Keys,
            ExpressionAttributeNames = Names,
            ExpressionAttributeValues = Values,
            TableName = TableName,
            ReturnValues = ReturnValue.ALL_NEW,
            UpdateExpression = BuildUpdateExpression(),
            ConditionExpression = BuildConditionExpression()
        };
    }
}
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using DynaMight.AtomicOperations;
using DynaMight.Criteria;

namespace DynaMight.Builders;

/// <summary>
/// Defines the Builder for the Atomic Operations
/// </summary>
public class AtomicBuilder : DynamoBuilder, IAtomicBuilder
{
    private readonly Dictionary<string, IList<IAtomicOperation>> _atomicOperations = new();

    /// <summary>
    /// Creates a new Atomic Builder
    /// </summary>
    private AtomicBuilder()
    {
    }

    /// <summary>
    /// Creates a new Atomic Builder
    /// </summary>
    public static IAtomicBuilder Create() => new AtomicBuilder();


    /// <inheritdoc />
    public IAtomicBuilder AddOperation(bool condition, Func<IAtomicOperation> func)
        => condition ? AddOperation(func()) : this;

    /// <inheritdoc />
    public IAtomicBuilder AddOperation(IAtomicOperation atomicOperation)
    {
        const string emptyName = "#";
        var nameExpression = atomicOperation.GetNameExpression();
        if (nameExpression.key == emptyName) // If the key is passed as a empty string
            return this;

        AddNameExpression(nameExpression);

        if (string.IsNullOrEmpty(atomicOperation.UpdateExpressionType))
            return this;

        if (!_atomicOperations.ContainsKey(atomicOperation.UpdateExpressionType))
            _atomicOperations.Add(atomicOperation.UpdateExpressionType, new List<IAtomicOperation>());

        var valueExpression = atomicOperation.GetValueExpression();
        if (Values.ContainsKey(valueExpression.key))
            return this;

        _atomicOperations[atomicOperation.UpdateExpressionType].Add(atomicOperation);
        if (atomicOperation.UpdateExpressionType != "REMOVE")
            AddValueExpression(atomicOperation.GetValueExpression());

        return this;
    }

    IAtomicBuilder IAtomicBuilder.SetKey<TK>(string name, TK value)
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

    /// <inheritdoc />
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
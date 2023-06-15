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

    /// <summary>
    /// Adds a new Operation into the builder if the condition is true
    /// </summary>
    /// <param name="condition">The condition that needs to be satisfied to add the operation</param>
    /// <param name="func">The operation that will be added into the list. It uses a Func, which will be normally a
    /// `() => condition`.</param>
    /// <returns>The AtomicBuilder (for a fluent API usage)</returns>
    public IAtomicBuilder AddOperation(bool condition, Func<IAtomicOperation> func)
        => condition ? AddOperation(func()) : this;

    /// <summary>
    /// Adds a new Operation into the builder
    /// </summary>
    /// <param name="atomicOperation">The operation that will be added into the list</param>
    /// <returns>The AtomicBuilder (for a fluent API usage)</returns>
    public IAtomicBuilder AddOperation(IAtomicOperation atomicOperation)
    {
        if (!AddNameExpression(atomicOperation.GetNameExpression()))
            return this;

        if (!_atomicOperations.ContainsKey(atomicOperation.UpdateExpressionType))
            _atomicOperations.Add(atomicOperation.UpdateExpressionType, new List<IAtomicOperation>());

        _atomicOperations[atomicOperation.UpdateExpressionType].Add(atomicOperation);
        AddValueExpression(atomicOperation.GetValueExpression());
        return this;
    }

    /// <summary>
    /// Sets the Key for the Builder. If the table has a Hash and Sort key, use the method twice, one of each key.
    /// </summary>
    /// <param name="name">The field's name that will be added. Recommended to use `nameof` function to avoid issues when renaming.</param>
    /// <param name="value">The key value</param>
    /// <typeparam name="TK">The type of the key (int, long, string, etc)</typeparam>
    /// <returns>The AtomicBuilder (for a fluent API usage)</returns>
    IAtomicBuilder IAtomicBuilder.SetKey<TK>(string name, TK value)
    {
        base.SetKey(name, value);
        return this;
    }

    /// <summary>
    /// Adds a new Criteria into the builder
    /// </summary>
    /// <param name="condition">The condition that needs to be satisfied to add the criteria</param>
    /// <param name="func">The criteria that will be added into the list. It uses a Func, which will be normally a
    /// `() => criteria`.</param>
    /// <returns>The AtomicBuilder (for a fluent API usage)</returns>
    IAtomicBuilder IAtomicBuilder.AddCriteria(bool condition, Func<IDynamoCriteria> func)
    {
        base.AddCriteria(condition, func);
        return this;
    }

    /// <summary>
    /// Adds a new Criteria into the builder
    /// </summary>
    /// <param name="criteria">The criteria that will be added into the builder</param>
    /// <returns>The AtomicBuilder (for a fluent API usage)</returns>
    IAtomicBuilder IAtomicBuilder.AddCriteria(IDynamoCriteria criteria)
    {
        base.AddCriteria(criteria);
        return this;
    }

    /// <summary>
    /// Defines if the Criteria will use parenthesis or not
    /// </summary>
    /// <returns>The AtomicBuilder (for a fluent API usage)</returns>
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

    /// <summary>
    /// Build the DynamoDB object that will be used to do the atomic operation. Called internally by the
    /// DynaMightContext.
    /// </summary>
    /// <returns>An `UpdateItemRequest` object</returns>
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
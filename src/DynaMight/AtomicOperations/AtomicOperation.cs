using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using DynaMight.Converters;

namespace DynaMight.AtomicOperations;

/// <summary>
/// Basic implementation for the AtomicOperation of Type T
/// </summary>
/// <typeparam name="T">The value's type</typeparam>
public abstract class AtomicOperation<T> : IAtomicOperation
{
    /// <inheritdoc />
    public virtual string UpdateExpressionType => "SET";
    /// <summary>
    /// Operation's field name
    /// </summary>
    protected string FieldName { get; init; } = default!;
    /// <summary>
    /// Operation's value
    /// </summary>
    protected T? Value { get; init; }
    /// <summary>
    /// The generated AttributeValue for the operation
    /// </summary>
    protected AttributeValue AttributeValue { get; init; } = default!;
    /// <summary>
    /// The pattern for the UpdateExpression
    /// </summary>
    protected abstract string Pattern { get; }

    /// <inheritdoc />
    public virtual string GetUpdateExpression() => string.Format(Pattern, FieldName);

    /// <inheritdoc />
    public virtual (string key, string value) GetNameExpression() => ($"#{FieldName}", FieldName);

    /// <inheritdoc />
    public virtual (string, AttributeValue, DynamoDBEntry?) GetValueExpression()
        => ($":{FieldName}", AttributeValue, DynamoValueConverter.ToDynamoDbEntry(Value));
}
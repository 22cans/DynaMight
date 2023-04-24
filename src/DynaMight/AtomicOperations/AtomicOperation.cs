using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using DynaMight.Converters;

namespace DynaMight.AtomicOperations;

public abstract class AtomicOperation<T> : IAtomicOperation
{
    public virtual string UpdateExpressionType => "SET";
    protected string FieldName { get; init; } = default!;
    protected T? Value { get; init; }
    protected AttributeValue AttributeValue { get; init; } = default!;
    protected abstract string Pattern { get; }

    public virtual string GetUpdateExpression() => string.Format(Pattern, FieldName);

    public virtual (string key, string value) GetNameExpression() => ($"#{FieldName}", FieldName);

    public virtual (string, AttributeValue, DynamoDBEntry?) GetValueExpression()
        => ($":{FieldName}", AttributeValue, AttributeValueConverter.ToDynamoDbEntry(Value));
}
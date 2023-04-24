using DynaMight.Converters;

namespace DynaMight.AtomicOperations;

public class AddAtomicOperation<T> : AtomicOperation<T>
{
    public override string UpdateExpressionType => "ADD";
    protected override string Pattern => "#{0} :{0}";

    public AddAtomicOperation(string fieldName, T value)
    {
        FieldName = fieldName;
        AttributeValue = DynamoValueConverter.From(value);
        Value = value;
    }

    public override string GetUpdateExpression() => string.Format(Pattern, FieldName);
}
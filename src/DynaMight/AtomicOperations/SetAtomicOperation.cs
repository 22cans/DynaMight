using DynaMight.Converters;

namespace DynaMight.AtomicOperations;

public class SetAtomicOperation<T> : AtomicOperation<T>
{
    protected override string Pattern => "#{0} = :{0}";

    public SetAtomicOperation(string fieldName, T value)
    {
        FieldName = fieldName;
        AttributeValue = AttributeValueConverter.From(value);
        Value = value;
    }

    public override string GetUpdateExpression() => string.Format(Pattern, FieldName);
}
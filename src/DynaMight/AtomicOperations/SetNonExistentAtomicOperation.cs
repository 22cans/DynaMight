namespace DynaMight.AtomicOperations;

public class SetNonExistentAtomicOperation<T> : AtomicOperation<T>
{
    protected override string Pattern => "#{0} = if_not_exists(#{0}, :{0})";

    public SetNonExistentAtomicOperation(string fieldName, T value)
    {
        FieldName = fieldName;
        AttributeValue = AttributeValueConverter.From(value);
        Value = value;
    }

    public override string GetUpdateExpression() => string.Format(Pattern, FieldName);
}
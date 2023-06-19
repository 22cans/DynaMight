using DynaMight.Converters;

namespace DynaMight.AtomicOperations;

public class SetNonExistentAtomicOperation<T> : AtomicOperation<T>
{
    protected override string Pattern => "#{0} = if_not_exists(#{0}, :{0})";

    /// <summary>
    /// Will do an atomic operation to set the value, but only if the field does not exist in DynamoDB
    /// </summary>
    /// <param name="fieldName">The field's name that will be added. Recommended to use `nameof` function to avoid issues when renaming.</param>
    /// <param name="value">The value that will be set, in case the field does not exist.</param>
    public SetNonExistentAtomicOperation(string fieldName, T value)
    {
        FieldName = fieldName;
        AttributeValue = DynamoValueConverter.ToAttributeValue(value);
        Value = value;
    }

    public override string GetUpdateExpression() => string.Format(Pattern, FieldName);
}
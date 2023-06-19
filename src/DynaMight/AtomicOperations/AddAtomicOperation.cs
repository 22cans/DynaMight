using DynaMight.Converters;

namespace DynaMight.AtomicOperations;

public class AddAtomicOperation<T> : AtomicOperation<T>
{
    public override string UpdateExpressionType => "ADD";
    protected override string Pattern => "#{0} :{0}";

    /// <summary>
    /// Creates an atomic operation that will use the ADD operation in the DynamoDB.
    /// </summary>
    /// <remarks>
    /// Use the ADD action in an update expression to add a new attribute and its values to an item.
    /// If the attribute already exists, the behavior of ADD depends on the attribute's data type:
    /// - If the attribute is a number, and the value you are adding is also a number, the value is mathematically added to the existing attribute. (If the value is a negative number, it is subtracted from the existing attribute.)
    /// - If the attribute is a set, and the value you are adding is also a set, the value is appended to the existing set.
    /// For more information, check https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Expressions.UpdateExpressions.html#Expressions.UpdateExpressions.ADD
    /// </remarks>
    /// <param name="fieldName">The field's name that will be added. Recommended to use `nameof` function to avoid issues when renaming.</param>
    /// <param name="value">The value that will be added.</param>
    public AddAtomicOperation(string fieldName, T value)
    {
        FieldName = fieldName;
        AttributeValue = DynamoValueConverter.ToAttributeValue(value);
        Value = value;
    }

    public override string GetUpdateExpression() => string.Format(Pattern, FieldName);
}
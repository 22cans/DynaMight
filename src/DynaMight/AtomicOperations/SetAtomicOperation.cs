using DynaMight.Converters;

namespace DynaMight.AtomicOperations;

/// <summary>
/// Creates an atomic operation that will use the SET operation in the DynamoDB.
/// </summary>
public class SetAtomicOperation<T> : AtomicOperation<T>
{
    /// <inheritdoc />
    protected override string Pattern => "#{0} = :{0}";

    /// <summary>
    /// Creates an atomic operation that will use the SET operation in the DynamoDB.
    /// </summary>
    /// <remarks>
    /// Use the SET action in an update expression to add attributes to an item. If any of these attributes already exists, they are overwritten by the new values.
    /// For more information, check https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Expressions.UpdateExpressions.html#Expressions.UpdateExpressions.SET
    /// </remarks>
    /// <param name="fieldName">The field's name that will be added. Recommended to use `nameof` function to avoid issues when renaming.</param>
    /// <param name="value">The value that will be set.</param>
    public SetAtomicOperation(string fieldName, T value)
    {
        FieldName = fieldName;
        AttributeValue = DynamoValueConverter.ToAttributeValue(value);
        Value = value;
    }
}
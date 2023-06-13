using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using DynaMight.Converters;

namespace DynaMight.AtomicOperations;

public class IncrementByAtomicOperation<T> : AtomicOperation<T>
{
    protected override string Pattern => "#{0} = #{0} + :{0}Increment";

    /// <summary>
    /// Creates an atomic operation that will use the SET operation in the DynamoDB with an addition operation (`field = field + value`).
    /// </summary>
    /// <remarks>
    /// If the field does not exist in the register, it will throw an exception. 
    /// </remarks>
    /// <param name="fieldName">The field's name that will be added. Recommended to use `nameof` function to avoid issues when renaming.</param>
    /// <param name="increment">The value that will be incremented.</param>
    public IncrementByAtomicOperation(string fieldName, T increment)
    {
        FieldName = fieldName;
        AttributeValue = new AttributeValue { N = increment!.ToString() };
        Value = increment;
    }

    public override (string, AttributeValue, DynamoDBEntry?) GetValueExpression()
        => ($":{FieldName}Increment", AttributeValue, DynamoValueConverter.ToDynamoDbEntry(Value));
}
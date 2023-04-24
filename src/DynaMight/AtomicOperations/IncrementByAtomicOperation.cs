using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using DynaMight.Converters;

namespace DynaMight.AtomicOperations;

public class IncrementByAtomicOperation<T> : AtomicOperation<T>
{
    protected override string Pattern => "#{0} = #{0} + :{0}Increment";

    public IncrementByAtomicOperation(string fieldName, T increment)
    {
        FieldName = fieldName;
        AttributeValue = new AttributeValue { N = increment!.ToString() };
        Value = increment;
    }

    public override (string, AttributeValue, DynamoDBEntry?) GetValueExpression()
        => ($":{FieldName}Increment", AttributeValue, DynamoValueConverter.ToDynamoDbEntry(Value));
}
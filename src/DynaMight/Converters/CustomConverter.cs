using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;

namespace DynaMight.Converters;

public class CustomConverter
{
    public virtual Func<object, AttributeValue> ToAttributeValue { get; init; } =
        v => new AttributeValue { N = v.ToString() };

    public virtual Func<AttributeValue, object?> ToObject { get; init; } = default!;
    public virtual Func<object, DynamoDBEntry?>? ToDynamoDbEntry { get; init; }
}
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;

namespace DynaMight.Converters;

/// <summary>
/// Defines a custom converter from/to DynamoDB objects
/// </summary>
public class CustomConverter
{
    /// <summary>
    /// Func that converts an object to AttributeValue
    /// </summary>
    public virtual Func<object, AttributeValue> ToAttributeValue { get; init; } =
        v => new AttributeValue { N = v.ToString() };
    /// <summary>
    /// Func that converts an AttributeValue to object
    /// </summary>
    public virtual Func<AttributeValue, object?> ToObject { get; init; } = default!;
    /// <summary>
    /// Func that converts an object to DynamoDBEntry
    /// </summary>
    public virtual Func<object, DynamoDBEntry?>? ToDynamoDbEntry { get; init; }
}
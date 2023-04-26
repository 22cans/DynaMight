using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;

namespace DynaMight.Converters;

public class CustomDynamoValueConverter
{
    internal static readonly Dictionary<Type, CustomDynamoValueConverter> CustomConverter = new();

    public static void AddCustomConverter<T>(Func<T, AttributeValue> attributeValue,
        Func<T, DynamoDBEntry?> dynamoDbEntry)
    {
        var converter = new CustomDynamoValueConverter<T>(attributeValue, dynamoDbEntry);
        if (CustomConverter.ContainsKey(typeof(T)))
            CustomConverter[typeof(T)] = converter;
        else
            CustomConverter.Add(typeof(T), converter);
    }
}

public class CustomDynamoValueConverter<T> : CustomDynamoValueConverter
{
    public readonly Func<T, AttributeValue> AttributeValue;
    public readonly Func<T, DynamoDBEntry?> DynamoDbEntry;

    internal CustomDynamoValueConverter(Func<T, AttributeValue> attributeValue, Func<T, DynamoDBEntry?> dynamoDbEntry)
    {
        AttributeValue = attributeValue;
        DynamoDbEntry = dynamoDbEntry;
    }
}
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;

namespace DynaMight.Converters;

public static class DynamoValueConverter
{
    public static AttributeValue From<T>(T value)
    {
        if (value == null || !CustomDynamoValueConverter.CustomConverter.ContainsKey(value.GetType()))
            return value switch
            {
                int or long or double or decimal => new AttributeValue { N = value.ToString() },
                bool b => new AttributeValue { BOOL = b },
                string s => new AttributeValue { S = s },
                List<string> ss => new AttributeValue { SS = ss },
                null => new AttributeValue { NULL = true },
                _ => throw new NotSupportedException(
                    $"The type '{typeof(T)}' is not supported by '{nameof(From)}'. Current value: {value}"),
            };

        if (CustomDynamoValueConverter.CustomConverter[value.GetType()] is not CustomDynamoValueConverter<T>
            converter)
            throw new NotSupportedException(
                $"The type '{typeof(T)}' is not supported by '{nameof(From)}'. Current value: {value}");

        return converter.AttributeValue(value);
    }

    public static DynamoDBEntry? ToDynamoDbEntry<T>(T value)
    {
        if (value == null || !CustomDynamoValueConverter.CustomConverter.ContainsKey(value.GetType()))
            return value switch
            {
                int v => v,
                long v => v,
                double v => v,
                decimal v => v,
                bool v => v,
                string v => v,
                null => null,
                List<string> v => v,
                _ => throw new NotSupportedException(
                    $"The type '{typeof(T)}' is not supported by '{nameof(ToDynamoDbEntry)}'. Current value: {value}"),
            };

        if (CustomDynamoValueConverter.CustomConverter[value.GetType()] is not CustomDynamoValueConverter<T>
            converter)
            throw new NotSupportedException(
                $"The type '{typeof(T)}' is not supported by '{nameof(ToDynamoDbEntry)}'. Current value: {value}");

        return converter.DynamoDbEntry(value);
    }
}
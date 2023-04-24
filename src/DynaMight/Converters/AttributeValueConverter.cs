using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using DynaMight.AtomicOperations;

namespace DynaMight.Converters;

public static class AttributeValueConverter
{
    private static readonly Dictionary<Type, Func<AttributeValue>> CustomDictionaryAttributeValue = new();
    private static readonly Dictionary<Type, Func<DynamoDBEntry>> CustomDictionaryDynamoDbEntries = new();

    public static void AddCustomType<T>(Func<AttributeValue> attributeValue, Func<DynamoDBEntry> dynamoDbEntry)
    {
        CustomDictionaryAttributeValue.Add(typeof(T), attributeValue);
        CustomDictionaryDynamoDbEntries.Add(typeof(T), dynamoDbEntry);
    }

    public static AttributeValue From<T>(T value)
    {
        if (value != null && CustomDictionaryAttributeValue.ContainsKey(value.GetType()))
            return CustomDictionaryAttributeValue[value.GetType()]();
        
        return value switch
        {
            int or long or double or decimal => new AttributeValue { N = value.ToString() },
            bool b => new AttributeValue { BOOL = b },
            string s => new AttributeValue { S = s },
            List<string> ss => new AttributeValue { SS = ss },
            //TODO: add on custom
            //RarityEnum r => new AttributeValue { N = ((int)r).ToString() },
            null => new AttributeValue { NULL = true },
            _ => throw new NotSupportedException(
                $"The type '{typeof(T)}' is not supported by '{nameof(SetAtomicOperation<T>)}'. Current value: {value}"),
        };
    }

    public static DynamoDBEntry? ToDynamoDbEntry<T>(T value)
    {
        if (value != null && CustomDictionaryDynamoDbEntries.ContainsKey(value.GetType()))
            return CustomDictionaryDynamoDbEntries[value.GetType()]();
        
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
            //TODO: add to custom on test
            // RarityEnum v => (int)v,
            _ => throw new NotSupportedException(
                $"The type '{typeof(T)}' is not supported by '{nameof(ToDynamoDbEntry)}'. Current value: {value}"),
        };
    }
}
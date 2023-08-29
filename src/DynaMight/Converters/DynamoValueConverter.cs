using System.Reflection;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;

namespace DynaMight.Converters;

/// <summary>
/// Converter logic from/to DynamoDB Values
/// </summary>
public static class DynamoValueConverter
{
    private static readonly Dictionary<Type, CustomConverter> Converters = new()
    {
        {
            typeof(int), new CustomConverter
            {
                ToObject = v => int.Parse(v.N),
                ToDynamoDbEntry = v => (int)v
            }
        },
        {
            typeof(long), new CustomConverter
            {
                ToObject = v => long.Parse(v.N),
                ToDynamoDbEntry = v => (long)v
            }
        },
        {
            typeof(decimal), new CustomConverter
            {
                ToObject = v => decimal.Parse(v.N),
                ToDynamoDbEntry = v => (decimal)v
            }
        },
        {
            typeof(double), new CustomConverter
            {
                ToObject = v => double.Parse(v.N),
                ToDynamoDbEntry = v => (double)v
            }
        },
        {
            typeof(string), new CustomConverter
            {
                ToAttributeValue = v => new AttributeValue { S = v.ToString() },
                ToObject = v => v.S,
                ToDynamoDbEntry = v => (string)v
            }
        },
        {
            typeof(bool), new CustomConverter
            {
                ToAttributeValue = v => new AttributeValue { BOOL = (bool)v },
                ToObject = v => v.BOOL,
                ToDynamoDbEntry = v => (bool)v
            }
        },
        {
            typeof(List<string>), new CustomConverter
            {
                ToAttributeValue = v => new AttributeValue { SS = (List<string>)v },
                ToObject = v => v.SS,
                ToDynamoDbEntry = v => (List<string>)v
            }
        },
    };

    /// <summary>
    /// Adds a custom convert from/to DynamoDB Values
    /// </summary>
    /// <param name="type">The C# type that will be converted</param>
    /// <param name="converter">A <see cref="CustomConverter"/> for the C# type</param>
    public static void Add(Type type, CustomConverter converter) => Converters[type] = converter;

    /// <summary>
    /// Searches the CustomConverters registered and execute the <see cref="CustomConverter.ToAttributeValue"/> function
    /// </summary>
    /// <param name="value">The current value to be converted</param>
    /// <returns>An AttributeValue</returns>
    /// <exception cref="NotSupportedException">If the type is not registered, this exception will be thrown</exception>
    public static AttributeValue ToAttributeValue(object? value)
    {
        if (value is null)
            return new AttributeValue { NULL = true };

        if (!Converters.ContainsKey(value.GetType()))
            throw new NotSupportedException(
                $"The type '{value.GetType()}' is not supported by '{nameof(ToAttributeValue)}'. Current value: {value}");

        var converter = Converters[value.GetType()];
        return converter.ToAttributeValue(value);
    }

    /// <summary>
    /// Converts a class object to a Dictionary of strings/AttributeValues
    /// </summary>
    /// <param name="value">The class to be converted</param>
    /// <typeparam name="T">The class' type</typeparam>
    /// <returns>Dictionary of strings/AttributeValues</returns>
    public static Dictionary<string, AttributeValue> ToClassAttributeValue<T>(T value)
    {
        var dictionary = new Dictionary<string, AttributeValue>();

        foreach (var property in typeof(T).GetProperties())
        {
            if (property.GetCustomAttributes(typeof(DynamoDBIgnoreAttribute)).Any())
                continue;

            var propValue = property.GetValue(value);
            var dynamoValue = ToAttributeValue(propValue);
            dictionary.Add(property.Name, dynamoValue);
        }

        return dictionary;
    }

    /// <summary>
    /// Searches the CustomConverters registered and execute the <see cref="CustomConverter.ToDynamoDbEntry"/> function
    /// </summary>
    /// /// <param name="value">The current value to be converted</param>
    /// <returns>A DynamoDbEntry</returns>
    /// <exception cref="NotSupportedException">If the type is not registered, this exception will be thrown</exception>
    public static DynamoDBEntry? ToDynamoDbEntry(object? value)
    {
        if (value is null)
            return null;

        if (!Converters.ContainsKey(value.GetType()))
            throw new NotSupportedException(
                $"The type '{value.GetType()}' is not supported by '{nameof(ToDynamoDbEntry)}'. Current value: {value}");

        var converter = Converters[value.GetType()];
        return converter.ToDynamoDbEntry?.Invoke(value);
    }

    /// <summary>
    /// Converts a Dictionary of strings/AttributeValue to a C# class
    /// </summary>
    /// <param name="dict">The Dictionary of strings/AttributeValue returned by DynamoDB</param>
    /// <typeparam name="T">C# class' type</typeparam>
    /// <returns>The constructed class based on the values from the dictionary</returns>
    /// <exception cref="KeyNotFoundException">If a required property for the class is not present in the dictionary</exception>
    /// <exception cref="NotSupportedException">If a property's type is not registered in the converters</exception>
    public static T To<T>(IReadOnlyDictionary<string, AttributeValue> dict) where T : new()
    {
        var obj = new T();

        foreach (var property in typeof(T).GetProperties())
        {
            if (property.GetCustomAttributes(typeof(DynamoDBIgnoreAttribute)).Any() || !property.CanWrite)
                continue;

            if (!dict.ContainsKey(property.Name))
            {
                //https://devblogs.microsoft.com/dotnet/announcing-net-6-preview-7/#libraries-reflection-apis-for-nullability-information
                var context = new NullabilityInfoContext();
                var propInfo = context.Create(property);
                if (propInfo.WriteState is not NullabilityState.Nullable)
                    throw new KeyNotFoundException(property.Name);

                continue;
            }

            var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

            if (!Converters.ContainsKey(propertyType))
                throw new NotSupportedException(
                    $"Type '{property.PropertyType}' is not yet supported by atomic deserialization.");

            var converter = Converters[propertyType];
            var attributeValue = dict[property.Name];

            property.SetValue(obj, converter.ToObject(attributeValue));
        }

        return obj;
    }
}
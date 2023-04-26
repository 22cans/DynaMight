using System.Reflection;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using DynaMight.Converters;

namespace DynaMight;

public static class DynaMightCustomConverter
{
    public static void LoadAndRegister()
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (var type in assembly.GetTypes())
            {
                if (type.GetCustomAttributes(typeof(DynaMightCustomConverterAttribute), true).Length <= 0)
                    continue;

                if (type.IsEnum)
                    RegisterEnum(type);
            }
        }
    }

    private static void RegisterEnum(Type enumType)
    {
        var method = typeof(DynaMightCustomConverter).GetMethod(nameof(RegisterEnum),
            BindingFlags.Public | BindingFlags.Static)!;
        method = method.MakeGenericMethod(enumType);
        method.Invoke(null, null);
    }

    public static void RegisterEnum<T>()
    {
        Register<T>((obj, prop, dict) =>
                prop.SetValue(obj, (T)(object)int.Parse(dict[prop.Name].N)),
            r => new AttributeValue { N = Convert.ToInt32(r).ToString() },
            r => Convert.ToInt32(r));
    }

    public static void Register<T>(
        Action<object, PropertyInfo, IReadOnlyDictionary<string, AttributeValue>> action,
        Func<T, AttributeValue> attributeValue,
        Func<T, DynamoDBEntry> dynamoDbEntry)
    {
        void NullableAction(object obj, PropertyInfo prop, IReadOnlyDictionary<string, AttributeValue> dict)
        {
            if (!dict.ContainsKey(prop.Name)) return;

            action(obj, prop, dict);
        }

        AttributeValueDictionaryConverter.AddCustomConverter<T>(action);
        AttributeValueDictionaryConverter.AddCustomConverter(typeof(T), NullableAction);
        CustomDynamoValueConverter.AddCustomConverter(attributeValue, dynamoDbEntry);
        CustomDynamoValueConverter.AddCustomConverter<T?>(attributeValue, dynamoDbEntry);
    }
}
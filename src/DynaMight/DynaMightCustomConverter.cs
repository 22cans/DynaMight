using System.Reflection;
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
                else if (type.IsClass)
                    RegisterClass(type);
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
        DynamoValueConverter.Add(typeof(T), new CustomConverter
        {
            ToAttributeValue = v => new AttributeValue { N = Convert.ToInt32(v).ToString() },
            ToObject = v => (T)(object)int.Parse(v.N),
            ToDynamoDbEntry = v => Convert.ToInt32(v)
        });
    }

    private static void RegisterClass(Type type)
    {
        var method = typeof(DynaMightCustomConverter).GetMethod(nameof(RegisterClass),
            BindingFlags.Public | BindingFlags.Static)!;
        method = method.MakeGenericMethod(type);
        method.Invoke(null, null);
    }

    public static void RegisterClass<T>() where T : new()
    {
        DynamoValueConverter.Add(typeof(T), new CustomConverter
        {
            ToAttributeValue = v => new AttributeValue { M = DynamoValueConverter.ToClassAttributeValue((T)v) },
            ToObject = v => DynamoValueConverter.To<T>(v.M)
        });
    }
}
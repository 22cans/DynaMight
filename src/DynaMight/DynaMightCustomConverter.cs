using System.Reflection;
using Amazon.DynamoDBv2.Model;
using DynaMight.Converters;

namespace DynaMight;

/// <summary>
/// Registers the <see cref="CustomConverter"/> defined by the application
/// </summary>
public static class DynaMightCustomConverter
{
    /// <summary>
    /// Loads all the assemblies and check the classes/enums that have a <see cref="DynaMightCustomConverterAttribute"/>
    /// and register their types in the CustomConverters
    /// </summary>
    /// <remarks>
    /// Enums will be registered as int conversions. If you want a custom implementation, don't use the Attribute for it.
    /// Instead, use the method <see cref="RegisterEnum{T}(CustomConverter)"/>, passing you custom implementation.
    /// </remarks>
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
            BindingFlags.Public | BindingFlags.Static, types: Type.EmptyTypes)!;
        method = method.MakeGenericMethod(enumType);
        method.Invoke(null, Array.Empty<object>());
    }

    /// <summary>
    /// Register a <see cref="CustomConverter"/> for the Enum with the default Int conversion
    /// </summary>
    /// <typeparam name="T">Enum's type</typeparam>
    public static void RegisterEnum<T>()
        => RegisterEnum<T>(new CustomConverter
        {
            ToAttributeValue = v => new AttributeValue { N = Convert.ToInt32(v).ToString() },
            ToObject = v => (T)(object)int.Parse(v.N),
            ToDynamoDbEntry = v => Convert.ToInt32(v)
        });

    /// <summary>
    /// Register a <see cref="CustomConverter"/> for the Enum
    /// </summary>
    /// <param name="converter">The <see cref="CustomConverter"/> for the enum.</param>
    /// <typeparam name="T">Enum's type</typeparam>
    public static void RegisterEnum<T>(CustomConverter converter)
        => DynamoValueConverter.Add(typeof(T), converter);


    private static void RegisterClass(Type type)
    {
        var method = typeof(DynaMightCustomConverter).GetMethod(nameof(RegisterClass),
            BindingFlags.Public | BindingFlags.Static)!;
        method = method.MakeGenericMethod(type);
        method.Invoke(null, null);
    }

    /// <summary>
    /// Registers a <see cref="CustomConverter"/> for the Class
    /// </summary>
    /// <typeparam name="T">Class' type</typeparam>
    public static void RegisterClass<T>() where T : new()
    {
        DynamoValueConverter.Add(typeof(T), new CustomConverter
        {
            ToAttributeValue = v => new AttributeValue { M = DynamoValueConverter.ToClassAttributeValue((T)v) },
            ToObject = v => DynamoValueConverter.To<T>(v.M)
        });
    }
}
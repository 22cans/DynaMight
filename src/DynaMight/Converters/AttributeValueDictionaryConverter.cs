using System.Reflection;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using DynaMight.Extensions;

namespace DynaMight.Converters;

public abstract class AttributeValueDictionaryConverter
{
    protected abstract void SetValue(object raw, PropertyInfo propertyInfo,
        IReadOnlyDictionary<string, AttributeValue> dict);

    private static readonly IDictionary<Type, AttributeValueDictionaryConverter> ValueSetters =
        new Dictionary<Type, AttributeValueDictionaryConverter>();

    public static void AddCustomConverter<T>(
        Action<object, PropertyInfo, IReadOnlyDictionary<string, AttributeValue>> action)
    {
        ValueSetters[typeof(T)] = new AttributeValueDictionaryConverter<T>(action);
    }
    
    public static void AddCustomConverter(Type customType,
        Action<object, PropertyInfo, IReadOnlyDictionary<string, AttributeValue>> action)
    {
        Type[] args = { customType };
        var nullable = typeof(Nullable<>).MakeGenericType(args);
        var attribute = typeof(AttributeValueDictionaryConverter<>).MakeGenericType(nullable);
        var t = Activator.CreateInstance(attribute, action);
        ValueSetters[nullable] = (AttributeValueDictionaryConverter)t! ?? throw new InvalidOperationException();
    }

    public static T ConvertFrom<T>(IReadOnlyDictionary<string, AttributeValue> dict) where T : new()
    {
        var obj = new T();

        foreach (var property in typeof(T).GetProperties())
        {
            if (property.GetCustomAttributes(typeof(DynamoDBIgnoreAttribute)).Any())
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

            if (AttributeValueDictionaryConverter<T>.DefaultSetters.TryGetValue(propertyType, out var setter))
            {
                setter(obj, property, dict);
                continue;
            }

            AttributeValueDictionaryConverter setValue;

            try
            {
                setValue = ValueSetters[propertyType] ??
                           throw new NotSupportedException(
                               $"Type '{property.PropertyType}' is not yet supported by atomic deserialization.");
            }
            catch (KeyNotFoundException e)
            {
                // Type is not supported by deserialization
                throw new NotSupportedException(
                    $"Type '{property.PropertyType}' is not yet supported by atomic deserialization.", e);
            }

            try
            {
                setValue.SetValue(obj, property, dict);
            }
            catch (KeyNotFoundException e)
            {
                throw new NotSupportedException(
                    $"Type '{property.PropertyType}' is not yet supported by atomic deserialization.", e);
            }
        }

        return obj;
    }
}

public class AttributeValueDictionaryConverter<T> : AttributeValueDictionaryConverter
{
    public static readonly IDictionary<Type, Action<T, PropertyInfo, IReadOnlyDictionary<string, AttributeValue>>>
        DefaultSetters;

    static AttributeValueDictionaryConverter()
    {
        DefaultSetters = new Dictionary<Type, Action<T, PropertyInfo, IReadOnlyDictionary<string, AttributeValue>>>
        {
            // Basic types
            { typeof(string), (obj, prop, dict) => prop.SetValue(obj, dict.Get<string?>(prop.Name, x => x.S)) },
            { typeof(bool), (obj, prop, dict) => prop.SetValue(obj, dict[prop.Name].BOOL) },
            { typeof(int), (obj, prop, dict) => prop.SetValue(obj, int.Parse(dict[prop.Name].N)) },
            { typeof(long), (obj, prop, dict) => prop.SetValue(obj, long.Parse(dict[prop.Name].N)) },
            { typeof(double), (obj, prop, dict) => prop.SetValue(obj, double.Parse(dict[prop.Name].N)) },
            { typeof(decimal), (obj, prop, dict) => prop.SetValue(obj, decimal.Parse(dict[prop.Name].N)) },
            { typeof(DateTime), (obj, prop, dict) => prop.SetValue(obj, DateTime.Parse(dict[prop.Name].S)) },

            { typeof(List<string>), (obj, prop, dict) => prop.SetValue(obj, dict[prop.Name].SS) },
        };
    }

    private readonly Action<object, PropertyInfo, IReadOnlyDictionary<string, AttributeValue>> _conversionAction;

    public AttributeValueDictionaryConverter(
        Action<object, PropertyInfo, IReadOnlyDictionary<string, AttributeValue>> conversionAction)
    {
        _conversionAction = conversionAction;
    }

    protected override void SetValue(object raw, PropertyInfo propertyInfo,
        IReadOnlyDictionary<string, AttributeValue> dict)
    {
        _conversionAction.Invoke(raw, propertyInfo, dict);
    }
}
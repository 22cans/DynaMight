using Amazon.DynamoDBv2.Model;

namespace DynaMight.Extensions;

public static class DictionaryExtensions
{
    public static void Add<TK, TV>(this Dictionary<TK, TV> dict, (TK key, TV value) tuple) where TK : notnull
        => dict.Add(tuple.key, tuple.value);
    
    public static T? Get<T>(this IReadOnlyDictionary<string, AttributeValue> dict, string field, Func<AttributeValue, T> action)
    {
        if (!dict.ContainsKey(field))
            return default;

        var item = dict[field];
        return action(item);
    }

    public static int? GetInt(this IReadOnlyDictionary<string, AttributeValue> dict, string field)
    {
        if (!dict.ContainsKey(field))
            return default;

        var item = dict[field];
        return int.Parse(item.N);
    }
}
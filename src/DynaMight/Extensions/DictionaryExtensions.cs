namespace DynaMight.Extensions;

public static class DictionaryExtensions
{
    public static void Add<TK, TV>(this Dictionary<TK, TV> dict, (TK key, TV value) tuple) where TK : notnull
        => dict.Add(tuple.key, tuple.value);
}
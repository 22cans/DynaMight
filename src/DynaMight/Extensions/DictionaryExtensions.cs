namespace DynaMight.Extensions;

/// <summary>
/// Dictionary Extensions Methods
/// </summary>
public static class DictionaryExtensions
{
    /// <summary>
    /// Adds a tuple Key-ValuePair into the dictionary
    /// </summary>
    /// <param name="dict">The dictionary that will receive the new values</param>
    /// <param name="tuple">Tuple containing a key and a value</param>
    /// <typeparam name="TK">The Key's type</typeparam>
    /// <typeparam name="TV">The Value's type</typeparam>
    public static void Add<TK, TV>(this Dictionary<TK, TV> dict, (TK key, TV value) tuple) where TK : notnull
        => dict.Add(tuple.key, tuple.value);
}
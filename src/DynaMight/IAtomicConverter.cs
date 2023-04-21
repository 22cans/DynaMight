using Amazon.DynamoDBv2.Model;

namespace DynaMight;

#pragma warning disable CA2252
public interface IAtomicConverter<out T>
{
    static abstract T ConvertFromAtomic(IReadOnlyDictionary<string, AttributeValue> dict);
}
#pragma warning restore CA2252
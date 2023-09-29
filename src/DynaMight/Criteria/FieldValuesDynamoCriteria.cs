using DynaMight.Builders;
using DynaMight.Converters;

namespace DynaMight.Criteria;

internal class FieldValuesDynamoCriteria<T> : DynamoCriteria
{
    private readonly IDictionary<string, T> _keyValues;

    public FieldValuesDynamoCriteria(string fieldName, IEnumerable<T> values)
    {
        _keyValues = values.ToDictionary(_ => $":{fieldName}_{Guid.NewGuid():N}", x => x);
    }

    public override void UseAtomicOperationBuilder(IDynamoBuilder builder)
    {
        base.UseAtomicOperationBuilder(builder);
        foreach (var keyValue in _keyValues)
        {
            builder.AddValueExpression(keyValue.Key, DynamoValueConverter.ToAttributeValue(keyValue.Value),
                DynamoValueConverter.ToDynamoDbEntry(keyValue.Value));
        }
    }

    public override string ToString()
        => $"({string.Join(", ", _keyValues.Keys.Select(key => $"{key}"))})";
}
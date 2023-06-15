using DynaMight.Builders;
using DynaMight.Converters;

namespace DynaMight.Criteria;

internal class FieldValueDynamoCriteria<T> : DynamoCriteria
{
    private readonly string _keyName;
    private readonly T _value;

    public FieldValueDynamoCriteria(string fieldName, T value)
    {
        _value = value;
        _keyName = $":{fieldName}_{Guid.NewGuid():N}";
    }

    public override void UseAtomicOperationBuilder(IDynamoBuilder builder)
    {
        base.UseAtomicOperationBuilder(builder);

        builder.AddValueExpression(_keyName, DynamoValueConverter.From(_value),
            DynamoValueConverter.ToDynamoDbEntry(_value));
    }

    public override string ToString()
        => _keyName;
}
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

        builder.AddValueExpression(_keyName, AttributeValueConverter.From(_value),
            AttributeValueConverter.ToDynamoDbEntry(_value));
    }

    public override string ToString()
        => _keyName;

    public override string ToString(bool useParenthesis)
        => ToString();
}
using DynaMight.Builders;

namespace DynaMight.Criteria;

internal class FieldNameDynamoCriteria : DynamoCriteria
{
    private readonly string _fieldName;

    public FieldNameDynamoCriteria(string fieldName)
    {
        _fieldName = fieldName;
    }

    public override void UseAtomicOperationBuilder(IDynamoBuilder builder)
    {
        base.UseAtomicOperationBuilder(builder);
        builder.AddNameExpression((key: $"#{_fieldName}", value: _fieldName));
    }

    public static implicit operator FieldNameDynamoCriteria(string fieldName)
        => new(fieldName);

    public override string ToString()
        => $"#{_fieldName}";

    public override string ToString(bool useParenthesis)
        => ToString();
}
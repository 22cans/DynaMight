using DynaMight.Builders;

namespace DynaMight.Criteria;

public abstract class UnaryFunctionDynamoCriteria : DynamoCriteria
{
    protected abstract string FunctionName { get; }

    private readonly string _fieldName;

    public UnaryFunctionDynamoCriteria(string fieldName)
    {
        _fieldName = fieldName;
    }

    public override void UseAtomicOperationBuilder(IDynamoBuilder builder)
    {
        base.UseAtomicOperationBuilder(builder);
        builder.AddNameExpression((key: $"#{_fieldName}", value: _fieldName));
    }

    public override string ToString() => $"{FunctionName}(#{_fieldName})";
}
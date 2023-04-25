using DynaMight.Builders;

namespace DynaMight.Criteria;

public abstract class UnaryFunctionDynamoCriteria : DynamoCriteria
{
    protected abstract string FunctionName { get; }

    protected readonly string FieldName;

    protected UnaryFunctionDynamoCriteria(string fieldName)
    {
        FieldName = fieldName;
    }

    public override void UseAtomicOperationBuilder(IDynamoBuilder builder)
    {
        base.UseAtomicOperationBuilder(builder);
        builder.AddNameExpression((key: $"#{FieldName}", value: FieldName));
    }

    public override string ToString() => $"{FunctionName}(#{FieldName})";
}
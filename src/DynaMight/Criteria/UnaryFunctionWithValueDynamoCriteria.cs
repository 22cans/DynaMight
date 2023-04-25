using DynaMight.Builders;

namespace DynaMight.Criteria;

public abstract class UnaryFunctionWithValueDynamoCriteria<T> : UnaryFunctionDynamoCriteria
{
    private readonly FieldValueDynamoCriteria<T> _value;

    protected UnaryFunctionWithValueDynamoCriteria(string fieldName, T value) : base(fieldName)
    {
        _value = new FieldValueDynamoCriteria<T>(fieldName, value);
    }

    public override void UseAtomicOperationBuilder(IDynamoBuilder builder)
    {
        base.UseAtomicOperationBuilder(builder);
        _value.UseAtomicOperationBuilder(builder);
    }
    
    public override string ToString() => $"{FunctionName}(#{FieldName}, {_value})";
}
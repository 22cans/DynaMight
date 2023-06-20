using DynaMight.Builders;

namespace DynaMight.Criteria;

/// <summary>
/// Criteria with unary criteria with value FUNCTION_NAME(FIELD_NAME, VALUE)
/// </summary>
public abstract class UnaryFunctionWithValueDynamoCriteria<T> : UnaryFunctionDynamoCriteria
{
    private readonly FieldValueDynamoCriteria<T> _value;

    /// <summary>
    /// Creates a criteria with unary criteria with value FUNCTION_NAME(FIELD_NAME, VALUE)
    /// </summary>
    /// <param name="fieldName">The Field's name</param>
    /// <param name="value">The function/criteria value</param>
    protected UnaryFunctionWithValueDynamoCriteria(string fieldName, T value) : base(fieldName)
    {
        _value = new FieldValueDynamoCriteria<T>(fieldName, value);
    }

    /// <inheritdoc />
    public override void UseAtomicOperationBuilder(IDynamoBuilder builder)
    {
        base.UseAtomicOperationBuilder(builder);
        _value.UseAtomicOperationBuilder(builder);
    }

    /// <inheritdoc />
    public override string ToString() => $"{FunctionName}(#{FieldName}, {_value})";
}
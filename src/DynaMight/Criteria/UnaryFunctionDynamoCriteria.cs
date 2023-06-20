using DynaMight.Builders;

namespace DynaMight.Criteria;

/// <summary>
/// Criteria with unary criteria FUNCTION_NAME(FIELD_NAME)
/// </summary>
public abstract class UnaryFunctionDynamoCriteria : DynamoCriteria
{
    /// <summary>
    /// The function to be used in the criteria
    /// </summary>
    protected abstract string FunctionName { get; }

    /// <summary>
    /// Field's name
    /// </summary>
    protected readonly string FieldName;

    /// <summary>
    /// Creates a criteria with unary criteria FUNCTION_NAME(FIELD_NAME)
    /// </summary>
    /// <param name="fieldName">The Field's name</param>
    protected UnaryFunctionDynamoCriteria(string fieldName)
    {
        FieldName = fieldName;
    }

    /// <inheritdoc />
    public override void UseAtomicOperationBuilder(IDynamoBuilder builder)
    {
        base.UseAtomicOperationBuilder(builder);
        builder.AddNameExpression((key: $"#{FieldName}", value: FieldName));
    }

    /// <inheritdoc />
    public override string ToString() => $"{FunctionName}(#{FieldName})";
}
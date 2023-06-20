using DynaMight.Builders;

namespace DynaMight.Criteria;

/// <summary>
/// Abstract implementation for Criteria
/// </summary>
public abstract class DynamoCriteria : IDynamoCriteria
{
    /// <inheritdoc />
    public virtual void UseAtomicOperationBuilder(IDynamoBuilder builder) { }

    /// <inheritdoc />
    public virtual string ToString(bool useParenthesis)
    {
        return useParenthesis ? $"({this})" : ToString()!;
    }
}
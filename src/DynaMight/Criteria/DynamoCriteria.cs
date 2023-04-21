using DynaMight.Builders;

namespace DynaMight.Criteria;

public abstract class DynamoCriteria : IDynamoCriteria
{
    public virtual void UseAtomicOperationBuilder(IDynamoBuilder builder) { }

    public virtual string ToString(bool useParenthesis)
    {
        return useParenthesis ? $"({this})" : ToString()!;
    }
}
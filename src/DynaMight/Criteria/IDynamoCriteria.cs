using DynaMight.Builders;

namespace DynaMight.Criteria;

public interface IDynamoCriteria
{
    void UseAtomicOperationBuilder(IDynamoBuilder builder);
    string ToString(bool useParenthesis);
}
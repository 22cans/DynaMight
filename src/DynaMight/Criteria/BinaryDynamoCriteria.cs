using DynaMight.Builders;

namespace DynaMight.Criteria;

public abstract class BinaryDynamoCriteria : DynamoCriteria
{
    protected abstract string Operator { get; }

    private readonly IDynamoCriteria _leftCriteria;
    private readonly IDynamoCriteria _rightCriteria;

    public BinaryDynamoCriteria(IDynamoCriteria leftCriteria, IDynamoCriteria rightCriteria)
    {
        _leftCriteria = leftCriteria;
        _rightCriteria = rightCriteria;
    }

    public override void UseAtomicOperationBuilder(IDynamoBuilder builder)
    {
        base.UseAtomicOperationBuilder(builder);

        _leftCriteria.UseAtomicOperationBuilder(builder);
        _rightCriteria.UseAtomicOperationBuilder(builder);
    }

    public override string ToString()
    {
        return $"{_leftCriteria} {Operator} {_rightCriteria}";
    }
}
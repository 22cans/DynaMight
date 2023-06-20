using DynaMight.Builders;

namespace DynaMight.Criteria;

/// <summary>
/// Criteria with binary logic LEFT OPERATOR RIGHT
/// </summary>
public abstract class BinaryDynamoCriteria : DynamoCriteria
{
    /// <summary>
    /// String that joins the two inner criteria
    /// </summary>
    protected abstract string Operator { get; }

    private readonly IDynamoCriteria _leftCriteria;
    private readonly IDynamoCriteria _rightCriteria;

    /// <summary>
    /// Creates a binary criteria, with two inner criteria, concatenated using the <see cref="Operator"/>
    /// </summary>
    /// <param name="leftCriteria">Left inner criteria</param>
    /// <param name="rightCriteria">Right inner criteria</param>
    protected BinaryDynamoCriteria(IDynamoCriteria leftCriteria, IDynamoCriteria rightCriteria)
    {
        _leftCriteria = leftCriteria;
        _rightCriteria = rightCriteria;
    }

    /// <inheritdoc />
    public override void UseAtomicOperationBuilder(IDynamoBuilder builder)
    {
        base.UseAtomicOperationBuilder(builder);

        _leftCriteria.UseAtomicOperationBuilder(builder);
        _rightCriteria.UseAtomicOperationBuilder(builder);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{_leftCriteria} {Operator} {_rightCriteria}";
    }
}
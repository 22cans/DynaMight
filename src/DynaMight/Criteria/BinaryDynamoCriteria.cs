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

    /// <summary>
    /// Left criteria for the binary operation
    /// </summary>
    protected readonly IDynamoCriteria LeftCriteria;
    
    /// <summary>
    /// Right criteria for the binary operation
    /// </summary>
    protected readonly IDynamoCriteria RightCriteria;

    /// <summary>
    /// Creates a binary criteria, with two inner criteria, concatenated using the <see cref="Operator"/>
    /// </summary>
    /// <param name="leftCriteria">Left inner criteria</param>
    /// <param name="rightCriteria">Right inner criteria</param>
    protected BinaryDynamoCriteria(IDynamoCriteria leftCriteria, IDynamoCriteria rightCriteria)
    {
        LeftCriteria = leftCriteria;
        RightCriteria = rightCriteria;
    }

    /// <inheritdoc />
    public override void UseAtomicOperationBuilder(IDynamoBuilder builder)
    {
        base.UseAtomicOperationBuilder(builder);

        LeftCriteria.UseAtomicOperationBuilder(builder);
        RightCriteria.UseAtomicOperationBuilder(builder);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{LeftCriteria} {Operator} {RightCriteria}";
    }
}
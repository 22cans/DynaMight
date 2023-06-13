namespace DynaMight.Criteria;

public class AndDynamoCriteria : BinaryDynamoCriteria
{
    protected override string Operator => "AND";

    /// <summary>
    /// Creates a AND condition for the filter's criteria.
    /// </summary>
    /// <param name="leftCriteria">The left criteria</param>
    /// <param name="rightCriteria">The right criteria</param>
    public AndDynamoCriteria(IDynamoCriteria leftCriteria, IDynamoCriteria rightCriteria)
        : base(leftCriteria, rightCriteria) { }
}
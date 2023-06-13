namespace DynaMight.Criteria;

public class OrDynamoCriteria : BinaryDynamoCriteria
{
    protected override string Operator => "OR";

    /// <summary>
    /// Creates an OR condition for the filter's criteria.
    /// </summary>
    /// <param name="leftCriteria">The left criteria</param>
    /// <param name="rightCriteria">The right criteria</param>
    public OrDynamoCriteria(IDynamoCriteria leftCriteria, IDynamoCriteria rightCriteria)
        : base(leftCriteria, rightCriteria)
    {
    }
}
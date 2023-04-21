namespace DynaMight.Criteria;

public class OrDynamoCriteria : BinaryDynamoCriteria
{
    protected override string Operator => "OR";

    public OrDynamoCriteria(IDynamoCriteria leftCriteria, IDynamoCriteria rightCriteria)
        : base(leftCriteria, rightCriteria) { }
}
namespace DynaMight.Criteria;

public class AndDynamoCriteria : BinaryDynamoCriteria
{
    protected override string Operator => "AND";

    public AndDynamoCriteria(IDynamoCriteria leftCriteria, IDynamoCriteria rightCriteria)
        : base(leftCriteria, rightCriteria) { }
}
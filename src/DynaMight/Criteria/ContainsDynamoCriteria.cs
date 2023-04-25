namespace DynaMight.Criteria;

public class ContainsDynamoCriteria : UnaryFunctionDynamoCriteria
{
    protected override string FunctionName => "contains";

    public ContainsDynamoCriteria(string fieldName) : base(fieldName) { }
}
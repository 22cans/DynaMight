namespace DynaMight.Criteria;

public class NotExistsDynamoCriteria : UnaryFunctionDynamoCriteria
{
    protected override string FunctionName => "attribute_not_exists";

    public NotExistsDynamoCriteria(string fieldName) : base(fieldName) { }
}
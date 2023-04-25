namespace DynaMight.Criteria;

public class ContainsDynamoCriteria<T> : UnaryFunctionWithValueDynamoCriteria<T>
{
    protected override string FunctionName => "contains";

    public ContainsDynamoCriteria(string fieldName, T value) : base(fieldName, value)
    {
    }
}
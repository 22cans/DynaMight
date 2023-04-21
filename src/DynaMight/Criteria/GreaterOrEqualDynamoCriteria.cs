namespace DynaMight.Criteria;

public class GreaterOrEqualDynamoCriteria<T> : BinaryDynamoCriteria
{
    public GreaterOrEqualDynamoCriteria(string fieldName, T value)
        : base(new FieldNameDynamoCriteria(fieldName), new FieldValueDynamoCriteria<T>(fieldName, value)) { }

    protected override string Operator => ">=";
}
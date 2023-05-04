namespace DynaMight.Criteria;

public class LessOrEqualDynamoCriteria<T> : BinaryDynamoCriteria
{
    public LessOrEqualDynamoCriteria(string fieldName, T value)
        : base(new FieldNameDynamoCriteria(fieldName), new FieldValueDynamoCriteria<T>(fieldName, value)) { }

    protected override string Operator => "<=";
}
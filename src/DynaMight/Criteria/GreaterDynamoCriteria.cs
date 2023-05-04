namespace DynaMight.Criteria;

public class GreaterDynamoCriteria<T> : BinaryDynamoCriteria
{
    public GreaterDynamoCriteria(string fieldName, T value)
        : base(new FieldNameDynamoCriteria(fieldName), new FieldValueDynamoCriteria<T>(fieldName, value)) { }

    protected override string Operator => ">";
}
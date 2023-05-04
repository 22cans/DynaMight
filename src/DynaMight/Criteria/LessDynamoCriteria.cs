namespace DynaMight.Criteria;

public class LessDynamoCriteria<T> : BinaryDynamoCriteria
{
    public LessDynamoCriteria(string fieldName, T value)
        : base(new FieldNameDynamoCriteria(fieldName), new FieldValueDynamoCriteria<T>(fieldName, value)) { }

    protected override string Operator => "<";
}
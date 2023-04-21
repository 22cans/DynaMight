namespace DynaMight.Criteria;

public class EqualDynamoCriteria<T> : BinaryDynamoCriteria
{
    public EqualDynamoCriteria(string fieldName, T value)
        : base(new FieldNameDynamoCriteria(fieldName), new FieldValueDynamoCriteria<T>(fieldName, value)) { }
    
    protected override string Operator => "=";
}
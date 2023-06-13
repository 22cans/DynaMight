namespace DynaMight.Criteria;

public class LessOrEqualDynamoCriteria<T> : BinaryDynamoCriteria
{
    /// <summary>
    /// Creates a `less than or equals (&lt;=)` criteria for the filter.
    /// </summary>
    /// <param name="fieldName">The field's name that will be added. Recommended to use `nameof` function to avoid issues when renaming.</param>
    /// <param name="value">The value that will be use for the criteria.</param>
    public LessOrEqualDynamoCriteria(string fieldName, T value)
        : base(new FieldNameDynamoCriteria(fieldName), new FieldValueDynamoCriteria<T>(fieldName, value)) { }

    protected override string Operator => "<=";
}
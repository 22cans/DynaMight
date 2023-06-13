namespace DynaMight.Criteria;

public class GreaterDynamoCriteria<T> : BinaryDynamoCriteria
{
    /// <summary>
    /// Creates a `greater than (>)` criteria for the filter.
    /// </summary>
    /// <param name="fieldName">The field's name that will be added. Recommended to use `nameof` function to avoid issues when renaming.</param>
    /// <param name="value">The value that will be use for the criteria.</param>
    public GreaterDynamoCriteria(string fieldName, T value)
        : base(new FieldNameDynamoCriteria(fieldName), new FieldValueDynamoCriteria<T>(fieldName, value)) { }

    protected override string Operator => ">";
}
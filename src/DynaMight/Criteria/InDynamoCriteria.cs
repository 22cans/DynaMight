namespace DynaMight.Criteria;

/// <summary>
/// Creates an `in` criteria for the filter.
/// </summary>
public class InDynamoCriteria<T> : BinaryDynamoCriteria
{
    /// <summary>
    /// Creates an `in` criteria for the filter.
    /// </summary>
    /// <param name="fieldName">The field's name that will be added. Recommended to use `nameof` function to avoid issues when renaming.</param>
    /// <param name="values">The values that will be used for the criteria.</param>
    public InDynamoCriteria(string fieldName, IEnumerable<T> values)
        : base(new FieldNameDynamoCriteria(fieldName), new FieldValuesDynamoCriteria<T>(fieldName, values))
    {
    }

    /// <inheritdoc />
    protected override string Operator => "IN";
}
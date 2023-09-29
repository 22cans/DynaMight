namespace DynaMight.Criteria;

/// <summary>
/// Creates a `contains` criteria for the filter.
/// </summary>
public class ContainsDynamoCriteria<T> : BinaryDynamoCriteria //: UnaryFunctionWithValueDynamoCriteria<T>
{
    /// <summary>
    /// Creates a `contains` criteria for the filter.
    /// </summary>
    /// <param name="fieldName">The field's name that will be added. Recommended to use `nameof` function to avoid issues when renaming.</param>
    /// <param name="value">The value that will be use for the criteria.</param>
    public ContainsDynamoCriteria(string fieldName, T value) 
        : base(new FieldNameDynamoCriteria(fieldName), new FieldValueDynamoCriteria<T>(fieldName, value))
    {
    }

    /// <inheritdoc />
    protected override string Operator => "contains";
    
    /// <inheritdoc />
    public override string ToString() => $"{Operator}({LeftCriteria}, {RightCriteria})";
}
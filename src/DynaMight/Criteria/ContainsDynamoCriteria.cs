namespace DynaMight.Criteria;

public class ContainsDynamoCriteria<T> : UnaryFunctionWithValueDynamoCriteria<T>
{
    protected override string FunctionName => "contains";

    /// <summary>
    /// Creates a `contains` criteria for the filter.
    /// </summary>
    /// <param name="fieldName">The field's name that will be added. Recommended to use `nameof` function to avoid issues when renaming.</param>
    /// <param name="value">The value that will be use for the criteria.</param>
    public ContainsDynamoCriteria(string fieldName, T value) : base(fieldName, value)
    {
    }
}
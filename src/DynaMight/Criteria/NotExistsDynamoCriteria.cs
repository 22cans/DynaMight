namespace DynaMight.Criteria;

/// <summary>
/// Creates a `not exists` criteria for the filter.
/// </summary>
public class NotExistsDynamoCriteria : UnaryFunctionDynamoCriteria
{
    /// <inheritdoc />
    protected override string FunctionName => "attribute_not_exists";

    /// <summary>
    /// Creates a `not exists` criteria for the filter.
    /// </summary>
    /// <remarks>
    /// This checks that the field does not exists in the register in DynamoDB.
    /// </remarks>
    /// <param name="fieldName">The field's name that will be added. Recommended to use `nameof` function to avoid issues when renaming.</param>
    public NotExistsDynamoCriteria(string fieldName) : base(fieldName) { }
}
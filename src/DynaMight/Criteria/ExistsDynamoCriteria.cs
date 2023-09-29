namespace DynaMight.Criteria;

/// <summary>
/// Creates an `exists` criteria for the filter.
/// </summary>
public class ExistsDynamoCriteria : UnaryDynamoCriteria
{
    /// <summary>
    /// Creates a `not exists` criteria for the filter.
    /// </summary>
    /// <remarks>
    /// This checks that the field does not exists in the register in DynamoDB.
    /// </remarks>
    /// <param name="fieldName">The field's name that will be added. Recommended to use `nameof` function to avoid issues when renaming.</param>
    public ExistsDynamoCriteria(string fieldName)
        : base(new FieldNameDynamoCriteria(fieldName), true)
    {
    }

    /// <inheritdoc />
    protected override string Operator => "attribute_exists";
}
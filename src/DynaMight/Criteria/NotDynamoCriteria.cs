namespace DynaMight.Criteria;

/// <summary>
/// Creates a `not` criteria for the filter.
/// </summary>
public class NotDynamoCriteria : UnaryDynamoCriteria
{
    /// <summary>
    /// Creates a `not` criteria for the filter.
    /// </summary>
    /// <param name="criteria">Criteria being negated</param>
    public NotDynamoCriteria(IDynamoCriteria criteria) : base(criteria, true)
    {
    }

    /// <inheritdoc />
    protected override string Operator => "NOT";
}
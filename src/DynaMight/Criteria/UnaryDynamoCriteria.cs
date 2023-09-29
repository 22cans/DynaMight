using DynaMight.Builders;

namespace DynaMight.Criteria;

/// <summary>
/// Criteria with unary criteria
/// </summary>
public abstract class UnaryDynamoCriteria : DynamoCriteria
{
    private readonly IDynamoCriteria _criteria;
    private readonly bool _useParenthesis;

    /// <summary>
    /// Unary operator
    /// </summary>
    protected abstract string Operator { get; }

    /// <summary>
    /// Creates a criteria with unary criteria
    /// </summary>
    /// <param name="criteria">The internal criteria being used</param>
    /// <param name="useParenthesis">Adds or not parenthesis around the criteria.</param>
    protected UnaryDynamoCriteria(IDynamoCriteria criteria, bool useParenthesis)
    {
        _criteria = criteria;
        _useParenthesis = useParenthesis;
    }

    /// <inheritdoc />
    public override void UseAtomicOperationBuilder(IDynamoBuilder builder)
    {
        base.UseAtomicOperationBuilder(builder);
        _criteria.UseAtomicOperationBuilder(builder);
    }

    /// <inheritdoc />
    public override string ToString() =>
        $"{Operator}{(_useParenthesis ? "" : " ")}{_criteria.ToString(_useParenthesis)}";
}
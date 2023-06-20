using DynaMight.Builders;

namespace DynaMight.Criteria;

/// <summary>
/// Basic interface for Criteria
/// </summary>
public interface IDynamoCriteria
{
    /// <summary>
    /// Adds the criteria into the AtomicBuilder, with its field's name and value
    /// </summary>
    /// <param name="builder"></param>
    void UseAtomicOperationBuilder(IDynamoBuilder builder);
    /// <summary>
    /// Converts the criteria to a string representation
    /// </summary>
    /// <param name="useParenthesis">Defines if it will use parenthesis or not</param>
    /// <returns>String representation of the criteria</returns>
    string ToString(bool useParenthesis);
}
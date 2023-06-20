using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;

namespace DynaMight.AtomicOperations;

/// <summary>
/// Interface for the Atomic Operation
/// </summary>
public interface IAtomicOperation
{
    /// <summary>
    /// The update expression type in DynamoDB
    /// </summary>
    string UpdateExpressionType { get; }
    /// <summary>
    /// Gets the Update Expression for Dynamo objects
    /// </summary>
    /// <returns>The Update Expression</returns>
    string GetUpdateExpression();
    /// <summary>
    /// Gets the field's name for the operation
    /// </summary>
    /// <returns>The Field's name</returns>
    (string key, string value) GetNameExpression();
    /// <summary>
    /// Gets the value's for the operation
    /// </summary>
    /// <returns>The value's value</returns>
    (string, AttributeValue AttributeValue, DynamoDBEntry?) GetValueExpression();
}
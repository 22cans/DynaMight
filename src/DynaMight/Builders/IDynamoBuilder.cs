using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;

namespace DynaMight.Builders;

/// <summary>
/// Basic interface for builders
/// </summary>
public interface IDynamoBuilder
{
    /// <summary>
    /// Adds a named expression in the builder
    /// </summary>
    /// <param name="tuple">A tuple containing the key and value</param>
    /// <returns>True if the field was add successfully. If false, it means that the field was not inserted (probably
    /// because it was already there).</returns>
    bool AddNameExpression((string key, string value) tuple);
    /// <summary>
    /// Adds a value expression in the builder
    /// </summary>
    /// <param name="tuple">A tuple containing the key, AttributeValue and DynamoDbEntry.</param>
    /// <seealso cref="o:AddValueExpression(string key, Amazon.DynamoDBv2.Model.AttributeValue value, Amazon.DynamoDBv2.DocumentModel.AddValueExpression? dynamoDbEntry)"/>
    void AddValueExpression((string key, AttributeValue value, DynamoDBEntry? dynamoDbEntry) tuple);
    /// <summary>
    /// Adds a value expression in the builder
    /// </summary>
    /// <param name="key">The key to the value expression</param>
    /// <param name="value">The AttributeValue for the expression</param>
    /// <param name="dynamoDbEntry">The DynamoDBEntry for the expression</param>
    void AddValueExpression(string key, AttributeValue value, DynamoDBEntry? dynamoDbEntry);
}
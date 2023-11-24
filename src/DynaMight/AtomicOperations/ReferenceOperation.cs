using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;

namespace DynaMight.AtomicOperations;

/// <summary>
/// Creates an reference entry for the atomic operation
/// </summary>
public class ReferenceOperation : IAtomicOperation
{
    /// <inheritdoc />
    public string UpdateExpressionType => string.Empty;
    private string FieldName { get; }

    /// <inheritdoc />
    public string GetUpdateExpression() => string.Empty;

    /// <inheritdoc />
    public (string key, string value) GetNameExpression() => ($"#{FieldName}", FieldName);

    /// <inheritdoc />
    public (string key, AttributeValue AttributeValue, DynamoDBEntry? dynamoDbEntry) GetValueExpression() => (string.Empty, null!, null);
    
    /// <summary>
    /// Creates an reference entry in the AtomicBuilder. Should be used with the CustomOperation
    /// </summary>
    /// <param name="fieldName">The field's name that will be added. Recommended to use `nameof` function to avoid issues when renaming.</param>
    public ReferenceOperation(string fieldName)
    {
        FieldName = fieldName;
    }
}
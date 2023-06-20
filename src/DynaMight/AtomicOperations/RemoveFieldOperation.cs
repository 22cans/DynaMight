using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;

namespace DynaMight.AtomicOperations;

/// <summary>
/// Removes a property from the register in the DynamoDB.
/// </summary>
public sealed class RemoveFieldOperation : IAtomicOperation
{
    /// <inheritdoc />
    public string UpdateExpressionType => "REMOVE";
    private string FieldName { get; }

    /// <inheritdoc />
    public string GetUpdateExpression() => $"#{FieldName}";

    /// <inheritdoc />
    public (string key, string value) GetNameExpression() => ($"#{FieldName}", FieldName);

    /// <inheritdoc />
    public (string, AttributeValue AttributeValue, DynamoDBEntry?) GetValueExpression() => (string.Empty, null!, null);

    /// <summary>
    /// Removes a property from the register in the DynamoDB.
    /// </summary>
    /// <remarks>
    /// Use the REMOVE action in an update expression to remove an attributes from an item in Amazon DynamoDB.
    /// For more information, check https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Expressions.UpdateExpressions.html#Expressions.UpdateExpressions.REMOVE
    /// </remarks>
    /// <param name="fieldName">The field's name that will be added. Recommended to use `nameof` function to avoid issues when renaming.</param>
    public RemoveFieldOperation(string fieldName)
    {
        FieldName = fieldName;
    }
}
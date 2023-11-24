using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;

namespace DynaMight.AtomicOperations;

/// <summary>
/// Creates a custom operation, that receives Func to execute the internal methods.
/// </summary>
public class CustomOperation : IAtomicOperation
{
    private readonly string _getUpdateExpression;
    private readonly (string, string) _getNameExpressionFunc;
    private readonly (string, AttributeValue, DynamoDBEntry?) _getValueExpressionFunc;

    /// <inheritdoc />
    public string UpdateExpressionType {get; }
    
    /// <inheritdoc />
    public string GetUpdateExpression() => _getUpdateExpression;

    /// <inheritdoc />
    public (string key, string value) GetNameExpression() => _getNameExpressionFunc;

    /// <inheritdoc />
    public (string key, AttributeValue AttributeValue, DynamoDBEntry? dynamoDbEntry) GetValueExpression() =>
        _getValueExpressionFunc;

    /// <summary>
    /// Creates a custom operation, that receives Func to execute the internal methods.
    /// </summary>
    /// <param name="updateExpressionType">Value for <see cref="UpdateExpressionType"/>.</param>
    /// <param name="getUpdateExpression">Value for <see cref="GetUpdateExpression"/>.</param>
    /// <param name="getNameExpressionFunc">Value for <see cref="GetNameExpression"/>.</param>
    /// <param name="getValueExpressionFunc">Value for <see cref="GetValueExpression"/>.</param>
    public CustomOperation(string updateExpressionType, string getUpdateExpression,
        (string, string) getNameExpressionFunc, (string, AttributeValue, DynamoDBEntry?) getValueExpressionFunc)
    {
        UpdateExpressionType = updateExpressionType;
        _getUpdateExpression = getUpdateExpression;
        _getNameExpressionFunc = getNameExpressionFunc;
        _getValueExpressionFunc = getValueExpressionFunc;
    }
}
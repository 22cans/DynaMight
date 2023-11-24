using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;

namespace DynaMight.AtomicOperations;

/// <summary>
/// Creates a custom operation, that receives Func to execute the internal methods.
/// </summary>
public class CustomOperation : IAtomicOperation
{
    private readonly Func<string> _updateExpressionTypeFunc;
    private readonly Func<string> _getUpdateExpressionFunc;
    private readonly Func<(string, string)> _getNameExpressionFunc;
    private readonly Func<(string, AttributeValue, DynamoDBEntry?)> _getValueExpressionFunc;

    /// <inheritdoc />
    public string UpdateExpressionType => _updateExpressionTypeFunc();
    
    /// <inheritdoc />
    public string GetUpdateExpression() => _getUpdateExpressionFunc();

    /// <inheritdoc />
    public (string key, string value) GetNameExpression() => _getNameExpressionFunc();

    /// <inheritdoc />
    public (string key, AttributeValue AttributeValue, DynamoDBEntry? dynamoDbEntry) GetValueExpression() =>
        _getValueExpressionFunc();

    /// <summary>
    /// Creates a custom operation, that receives Func to execute the internal methods.
    /// </summary>
    /// <param name="updateExpressionTypeFunc">Func that defines <see cref="UpdateExpressionType"/>.</param>
    /// <param name="getUpdateExpressionFunc">Func that defines <see cref="GetUpdateExpression"/>.</param>
    /// <param name="getNameExpressionFunc">Func that defines <see cref="GetNameExpression"/>.</param>
    /// <param name="getValueExpressionFunc">Func that defines <see cref="GetValueExpression"/>.</param>
    public CustomOperation(Func<string> updateExpressionTypeFunc, Func<string> getUpdateExpressionFunc,
        Func<(string, string)> getNameExpressionFunc, Func<(string, AttributeValue, DynamoDBEntry?)> getValueExpressionFunc)
    {
        _updateExpressionTypeFunc = updateExpressionTypeFunc;
        _getUpdateExpressionFunc = getUpdateExpressionFunc;
        _getNameExpressionFunc = getNameExpressionFunc;
        _getValueExpressionFunc = getValueExpressionFunc;
    }
}
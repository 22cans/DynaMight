using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;

namespace DynaMight.AtomicOperations;

public interface IAtomicOperation
{
    string UpdateExpressionType { get; }
    string GetUpdateExpression();
    (string key, string value) GetNameExpression();
    (string, AttributeValue AttributeValue, DynamoDBEntry?) GetValueExpression();
}
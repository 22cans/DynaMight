using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;

namespace DynaMight.Builders;

public interface IDynamoBuilder
{
    void AddNameExpression((string key, string value) tuple);
    void AddValueExpression((string key, AttributeValue value, DynamoDBEntry? dynamoDbEntry) tuple);
    void AddValueExpression(string key, AttributeValue value, DynamoDBEntry? dynamoDbEntry);
}
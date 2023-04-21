using System.Reflection;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using DynaMight.Criteria;
using DynaMight.Extensions;

namespace DynaMight.Builders;

public abstract class DynamoBuilder : IDynamoBuilder
{
    protected readonly Dictionary<string, string> Names = new();
    protected readonly Dictionary<string, AttributeValue> Values = new();
    protected readonly Dictionary<string, DynamoDBEntry?> Entries = new();
    protected readonly Dictionary<string, AttributeValue> Keys = new();
    protected string TableName = string.Empty;
    private bool _useParenthesis;
    private IDynamoCriteria? _dynamoCriteria;

    public void AddNameExpression((string key, string value) tuple)
    {
        if (!Names.ContainsKey(tuple.key))
        {
            Names.Add(tuple);
        }
    }

    public void AddValueExpression((string key, AttributeValue value, DynamoDBEntry? dynamoDbEntry) tuple)
        => AddValueExpression(tuple.key, tuple.value, tuple.dynamoDbEntry);

    public void AddValueExpression(string key, AttributeValue value, DynamoDBEntry? dynamoDbEntry)
    {
        if (Values.ContainsKey(key))
            return;

        Values.Add(key, value);
        Entries.Add(key, dynamoDbEntry);
    }

    protected void SetKey<TK>(string name, TK value)
    {
        Keys.Add(name, AttributeValueConverter.From(value));
    }

    protected void UseParenthesis() => _useParenthesis = true;

    protected void AddCriteria(bool condition, Func<IDynamoCriteria> func)
    {
        if (condition)
            AddCriteria(func());
    }

    protected void AddCriteria(IDynamoCriteria criteria)
    {
        _dynamoCriteria = criteria;
        _dynamoCriteria.UseAtomicOperationBuilder(this);
    }

    protected void SetTableName<T>(DynamoDBContextConfig dynamoConfig)
    {
        var attribute = (DynamoDBTableAttribute)typeof(T).GetCustomAttribute(typeof(DynamoDBTableAttribute))!;

        TableName = $"{dynamoConfig.TableNamePrefix ?? string.Empty}{attribute.TableName}";
    }

    protected string? BuildConditionExpression() => _dynamoCriteria?.ToString(_useParenthesis);
}
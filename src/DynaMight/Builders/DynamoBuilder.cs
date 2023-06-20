using System.Reflection;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using DynaMight.Converters;
using DynaMight.Criteria;
using DynaMight.Extensions;

namespace DynaMight.Builders;

/// <summary>
/// Abstract implementation for the builders
/// </summary>
public abstract class DynamoBuilder : IDynamoBuilder
{
    /// <summary>
    /// Holds all the fields names and their references
    /// </summary>
    protected readonly Dictionary<string, string> Names = new();
    /// <summary>
    /// Holds the AttributeValues references
    /// </summary>
    protected readonly Dictionary<string, AttributeValue> Values = new();
    /// <summary>
    /// Holds the DynamoDbEntries references
    /// </summary>
    protected readonly Dictionary<string, DynamoDBEntry?> Entries = new();
    /// <summary>
    /// Holds the AttributeValues references for the Keys
    /// </summary>
    protected readonly Dictionary<string, AttributeValue> Keys = new();
    /// <summary>
    /// DynamoDB Table's name
    /// </summary>
    protected string TableName = string.Empty;
    private bool _useParenthesis;
    private IDynamoCriteria? _dynamoCriteria;

    /// <inheritdoc />
    public bool AddNameExpression((string key, string value) tuple)
    {
        if (tuple.key.Length == 1 || Names.ContainsKey(tuple.key))
            return false;
        
        Names.Add(tuple);
        return true;
    }

    /// <inheritdoc />
    public void AddValueExpression((string key, AttributeValue value, DynamoDBEntry? dynamoDbEntry) tuple)
        => AddValueExpression(tuple.key, tuple.value, tuple.dynamoDbEntry);

    /// <inheritdoc />
    public void AddValueExpression(string key, AttributeValue value, DynamoDBEntry? dynamoDbEntry)
    {
        Values.Add(key, value);
        Entries.Add(key, dynamoDbEntry);
    }

    /// <summary>
    /// Sets a key for the builder
    /// </summary>
    /// <param name="name">Field's name. Recommended to use `nameof` function to avoid issues when renaming.</param>
    /// <param name="value">The key's value</param>
    /// <typeparam name="TK">The key's type</typeparam>
    protected void SetKey<TK>(string name, TK value)
    {
        Keys.Add(name, DynamoValueConverter.ToAttributeValue(value));
    }

    /// <summary>
    /// Defines if the build expression will use parenthesis when creating the build expression.
    /// Recommended to set True if you are using several AND/OR criteria
    /// </summary>
    protected void UseParenthesis() => _useParenthesis = true;

    /// <summary>
    /// Adds a new Criteria into the builder
    /// </summary>
    /// <param name="condition">The condition that needs to be satisfied to add the criteria</param>
    /// <param name="func">The criteria that will be added into the list. It uses a Func, which will be normally a
    /// `() => criteria`.</param>
    /// <returns>The AtomicBuilder (for a fluent API usage)</returns>
    protected void AddCriteria(bool condition, Func<IDynamoCriteria> func)
    {
        if (condition)
            AddCriteria(func());
    }

    /// <summary>
    /// Adds a new Criteria into the builder
    /// </summary>
    /// <param name="criteria">The criteria that will be added into the builder</param>
    /// <returns>The AtomicBuilder (for a fluent API usage)</returns>
    protected void AddCriteria(IDynamoCriteria criteria)
    {
        _dynamoCriteria = criteria;
        _dynamoCriteria.UseAtomicOperationBuilder(this);
    }

    /// <summary>
    /// Set the Table's name, based on the DynamoDBContextConfig and the DynamoDBTable attribute
    /// </summary>
    /// <param name="dynamoConfig">The DynamoDBContextConfig used by the application</param>
    /// <typeparam name="T">The table's type in the C# code</typeparam>
    protected void SetTableName<T>(DynamoDBContextConfig dynamoConfig)
    {
        var attribute = (DynamoDBTableAttribute)typeof(T).GetCustomAttribute(typeof(DynamoDBTableAttribute))!;

        TableName = $"{dynamoConfig.TableNamePrefix ?? string.Empty}{attribute.TableName}";
    }

    /// <summary>
    /// Builds the condition expression
    /// </summary>
    /// <returns>String representation for the condition/criteria set in the builder</returns>
    protected string? BuildConditionExpression() => _dynamoCriteria?.ToString(_useParenthesis);
}
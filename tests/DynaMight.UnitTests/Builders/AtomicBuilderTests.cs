using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using DynaMight.Builders;
using DynaMight.Criteria;
using FluentAssertions;

namespace DynaMight.UnitTests.Builders;

public class AtomicBuilderTests
{
    private const string FieldName = "field";
    private const string FieldValue = "value";
    private const string SecondFieldName = "field2";
    private const long SecondFieldValue = 638212928431203164;

    [Fact]
    public void Create()
    {
        var builder = AtomicBuilder.Create();
        builder.Should().NotBeNull();
    }

    [Fact]
    public void AddCriteria()
    {
        var criteria = new EqualDynamoCriteria<string>(FieldName, FieldValue);
        var builder = AtomicBuilder
            .Create()
            .AddCriteria(criteria);

        var config = builder.Build();
        config.ConditionExpression.Should().MatchRegex($"\\#{FieldName} = :{FieldName}_.{{32}}");

        config.ExpressionAttributeNames.Should().NotBeNull();
        config.ExpressionAttributeNames.Count.Should().Be(1);
        config.ExpressionAttributeNames.Keys.Should().Contain($"#{FieldName}");
        config.ExpressionAttributeNames.Values.Should().Contain($"{FieldName}");

        config.ExpressionAttributeValues.Should().NotBeNull();
        config.ExpressionAttributeValues.Count.Should().Be(1);
        config.ExpressionAttributeValues.Keys.First().Should().MatchRegex($":{FieldName}_.{{32}}");
        config.ExpressionAttributeValues.Values.First().Should().BeEquivalentTo(new AttributeValue { S = FieldValue });
    }

    [Fact]
    public void AddCriteria_WithConditionTrue()
    {
        var criteria = new EqualDynamoCriteria<string>(FieldName, FieldValue);
        var builder = AtomicBuilder
            .Create()
            .AddCriteria(true, () => criteria);

        var config = builder.Build();
        config.ConditionExpression.Should().MatchRegex($"\\#{FieldName} = :{FieldName}_.{{32}}");

        config.ExpressionAttributeNames.Should().NotBeNull();
        config.ExpressionAttributeNames.Count.Should().Be(1);
        config.ExpressionAttributeNames.Keys.Should().Contain($"#{FieldName}");
        config.ExpressionAttributeNames.Values.Should().Contain($"{FieldName}");

        config.ExpressionAttributeValues.Should().NotBeNull();
        config.ExpressionAttributeValues.Count.Should().Be(1);
        config.ExpressionAttributeValues.Keys.First().Should().MatchRegex($":{FieldName}_.{{32}}");
        config.ExpressionAttributeValues.Values.First().Should().BeEquivalentTo(new AttributeValue { S = FieldValue });
    }

    [Fact]
    public void AddCriteria_WithConditionFalse()
    {
        var criteria = new EqualDynamoCriteria<string>(FieldName, FieldValue);
        var builder = AtomicBuilder
            .Create()
            .AddCriteria(false, () => criteria);

        var config = builder.Build();
        config.ConditionExpression.Should().BeNullOrEmpty();

        config.ExpressionAttributeNames.Should().NotBeNull();
        config.ExpressionAttributeNames.Count.Should().Be(0);

        config.ExpressionAttributeValues.Should().NotBeNull();
        config.ExpressionAttributeValues.Count.Should().Be(0);
    }

    [Fact]
    public void SetKey()
    {
        var builder = AtomicBuilder
            .Create()
            .SetKey(FieldName, FieldValue)
            .SetKey(SecondFieldName, SecondFieldValue);

        var config = builder.Build();

        config.Should().NotBeNull();

        var expectedConditions = new Dictionary<string, AttributeValue>
        {
            { FieldName, new() { S = FieldValue } },
            { SecondFieldName, new() { N = SecondFieldValue.ToString() } }
        };

        config.Key.Should().BeEquivalentTo(expectedConditions);
    }

    [Fact]
    public void UseParenthesis()
    {
        var left = new EqualDynamoCriteria<string>(FieldName, FieldValue);
        var right = new EqualDynamoCriteria<long>(SecondFieldName, SecondFieldValue);
        var criteria = new AndDynamoCriteria(left, right);

        var builder = AtomicBuilder
            .Create()
            .AddCriteria(criteria)
            .UseParenthesis();

        var config = builder.Build();
        config.ConditionExpression.Should().MatchRegex(
            $"\\(\\#{FieldName} = :{FieldName}_.{{32}} AND #{SecondFieldName} = :{SecondFieldName}_.{{32}}\\)");
    }

    [Fact]
    public void SetTableName()
    {
        var dynamoConfig = new DynamoDBContextConfig()
        {
            TableNamePrefix = "22cans_"
        };
        var builder = AtomicBuilder
            .Create()
            .SetTableName<DynaMightTestClass>(dynamoConfig);

        var config = builder.Build();
        config.TableName.Should().Be("22cans_DynaMightTestClass");
    }
}
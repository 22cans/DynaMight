using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using DynaMight.Builders;
using DynaMight.Criteria;
using FluentAssertions;

namespace DynaMight.UnitTests.Builders;

public class QueryBuilderTests
{
    private const string FieldName = "field";
    private const string FieldValue = "value";
    private const string SecondFieldName = "field2";
    private const long SecondFieldValue = 638212928431203164;

    [Fact]
    public void Create()
    {
        var builder = QueryBuilder.Create();
        builder.Should().NotBeNull();
    }

    [Fact]
    public void AddCriteria()
    {
        var criteria = new EqualDynamoCriteria<string>(FieldName, FieldValue);
        var builder = QueryBuilder
            .Create()
            .AddCriteria(criteria);

        var config = builder.Build(true);
        config.FilterExpression.ExpressionStatement.Should().MatchRegex($"\\#{FieldName} = :{FieldName}_.{{32}}");

        config.FilterExpression.ExpressionAttributeNames.Should().NotBeNull();
        config.FilterExpression.ExpressionAttributeNames.Count.Should().Be(1);
        config.FilterExpression.ExpressionAttributeNames.Keys.Should().Contain($"#{FieldName}");
        config.FilterExpression.ExpressionAttributeNames.Values.Should().Contain($"{FieldName}");

        config.FilterExpression.ExpressionAttributeValues.Should().NotBeNull();
        config.FilterExpression.ExpressionAttributeValues.Count.Should().Be(1);
        config.FilterExpression.ExpressionAttributeValues.Keys.First().Should().MatchRegex($":{FieldName}_.{{32}}");
        config.FilterExpression.ExpressionAttributeValues.Values.First().AsString().Should().Be($"{FieldValue}");
    }

    [Fact]
    public void AddCriteria_WithConditionTrue()
    {
        var criteria = new EqualDynamoCriteria<string>(FieldName, FieldValue);
        var builder = QueryBuilder
            .Create()
            .AddCriteria(true, () => criteria);

        var config = builder.Build(true);
        config.FilterExpression.ExpressionStatement.Should().MatchRegex($"\\#{FieldName} = :{FieldName}_.{{32}}");

        config.FilterExpression.ExpressionAttributeNames.Should().BeEquivalentTo(new Dictionary<string, string>
        {
            { $"#{FieldName}", FieldName }
        });

        config.FilterExpression.ExpressionAttributeValues.Should().NotBeNull();
        config.FilterExpression.ExpressionAttributeValues.Count.Should().Be(1);
        config.FilterExpression.ExpressionAttributeValues.Keys.First().Should().MatchRegex($":{FieldName}_.{{32}}");
        config.FilterExpression.ExpressionAttributeValues.Values.First().AsString().Should().Be($"{FieldValue}");
    }

    [Fact]
    public void AddCriteria_WithConditionFalse()
    {
        var criteria = new EqualDynamoCriteria<string>(FieldName, FieldValue);
        var builder = QueryBuilder
            .Create()
            .AddCriteria(false, () => criteria);

        var config = builder.Build(true);
        config.FilterExpression.ExpressionStatement.Should().BeNullOrEmpty();

        config.FilterExpression.ExpressionAttributeNames.Should().NotBeNull();
        config.FilterExpression.ExpressionAttributeNames.Count.Should().Be(0);

        config.FilterExpression.ExpressionAttributeValues.Should().NotBeNull();
        config.FilterExpression.ExpressionAttributeValues.Count.Should().Be(0);
    }

    [Fact]
    public void AddUnaryCriteria()
    {
        var criteria = new NotExistsDynamoCriteria(FieldName);
        var builder = QueryBuilder
            .Create()
            .AddCriteria(true, () => criteria);

        var config = builder.Build(true);
        config.FilterExpression.ExpressionStatement.Should().MatchRegex($"attribute_not_exists\\(\\#{FieldName}\\)");

        config.FilterExpression.ExpressionAttributeNames.Should().BeEquivalentTo(new Dictionary<string, string>
        {
            { $"#{FieldName}", FieldName }
        });

        config.FilterExpression.ExpressionAttributeValues.Should().BeNullOrEmpty();
    }

    [Fact]
    public void AddUnaryWithValueCriteria()
    {
        var criteria = new ContainsDynamoCriteria<string>(FieldName, FieldValue);
        var builder = QueryBuilder
            .Create()
            .AddCriteria(true, () => criteria);

        var config = builder.Build(true);
        config.FilterExpression.ExpressionStatement.Should()
            .MatchRegex($"contains\\(\\#{FieldName}, :{FieldName}_.{{32}}\\)");

        config.FilterExpression.ExpressionAttributeNames.Should().BeEquivalentTo(new Dictionary<string, string>
        {
            { $"#{FieldName}", FieldName }
        });

        config.FilterExpression.ExpressionAttributeValues.Should().NotBeNull();
        config.FilterExpression.ExpressionAttributeValues.Count.Should().Be(1);
        config.FilterExpression.ExpressionAttributeValues.Keys.First().Should().MatchRegex($":{FieldName}_.{{32}}");
        config.FilterExpression.ExpressionAttributeValues.Values.First().AsString().Should().Be($"{FieldValue}");
    }

    [Fact]
    public void SetKey()
    {
        var builder = QueryBuilder
            .Create()
            .SetKey(FieldName, FieldValue)
            .SetKey(SecondFieldName, SecondFieldValue);

        var config = builder.Build(true);

        config.Should().NotBeNull();

        var expectedConditions = new Dictionary<string, Condition>()
        {
            {
                FieldName, new()
                {
                    ComparisonOperator = ComparisonOperator.EQ,
                    AttributeValueList = new List<AttributeValue> { new() { S = FieldValue } }
                }
            },
            {
                SecondFieldName, new()
                {
                    ComparisonOperator = ComparisonOperator.EQ,
                    AttributeValueList = new List<AttributeValue> { new() { N = SecondFieldValue.ToString() } }
                }
            },
        };

        var conditions = config.Filter.ToConditions();
        conditions.Should().BeEquivalentTo(expectedConditions);
    }

    [Fact]
    public void UseParenthesis()
    {
        var left = new EqualDynamoCriteria<string>(FieldName, FieldValue);
        var right = new EqualDynamoCriteria<long>(SecondFieldName, SecondFieldValue);
        var criteria = new AndDynamoCriteria(left, right);

        var builder = QueryBuilder
            .Create()
            .AddCriteria(criteria)
            .UseParenthesis();

        var config = builder.Build(true);
        config.FilterExpression.ExpressionStatement.Should().MatchRegex(
            $"\\(\\#{FieldName} = :{FieldName}_.{{32}} AND #{SecondFieldName} = :{SecondFieldName}_.{{32}}\\)");
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData(10, null)]
    [InlineData(null, "next_page")]
    [InlineData(10, "next_page")]
    public void SetPage(int? pageSize, string? pageToken)
    {
        var builder = QueryBuilder
            .Create()
            .SetPage(pageSize, pageToken);

        var config = builder.Build(true);
        config.Should().NotBeNull();
        config.Limit.Should().Be(pageSize ?? IQueryBuilder.DefaultPageSize);
        config.PaginationToken.Should().Be(pageToken);
    }

    [Fact]
    public void OrderByDescending_NotCalled()
    {
        var builder = QueryBuilder
            .Create();

        var config = builder.Build(true);
        config.Should().NotBeNull();
        config.BackwardSearch.Should().BeFalse();
    }

    [Fact]
    public void OrderByDescending_Called()
    {
        var builder = QueryBuilder
            .Create()
            .OrderByDescending();

        var config = builder.Build(true);
        config.Should().NotBeNull();
        config.BackwardSearch.Should().BeTrue();
    }

    [Fact]
    public void SetIndex_NotCalled()
    {
        var builder = QueryBuilder
            .Create();

        var config = builder.Build(true);
        config.Should().NotBeNull();
        config.IndexName.Should().BeNullOrEmpty();
    }

    [Fact]
    public void SetIndex_Called()
    {
        const string index = "SecondaryIndex";
        var builder = QueryBuilder
            .Create()
            .SetIndexName(index);

        var config = builder.Build(true);
        config.Should().NotBeNull();
        config.IndexName.Should().Be(index);
    }
}
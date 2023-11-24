using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using DynaMight.AtomicOperations;
using DynaMight.Builders;
using DynaMight.Converters;
using DynaMight.Criteria;
using FluentAssertions;

namespace DynaMight.UnitTests.Builders;

public class AtomicBuilderTests
{
    private const string Field1Name = "field";
    private const string Field1Value = "value";
    private const string Field2Name = "field2";
    private const long Field2Value = 638212928431203164;
    private const string Field3Name = "field3";
    private const int Field3Value = 0;

    [Fact]
    public void Create()
    {
        var builder = AtomicBuilder.Create();
        builder.Should().NotBeNull();
    }

    [Fact]
    public void AddCriteria()
    {
        var criteria = new EqualDynamoCriteria<string>(Field1Name, Field1Value);
        var builder = AtomicBuilder
            .Create()
            .AddCriteria(criteria);

        var config = builder.Build();
        config.ConditionExpression.Should().MatchRegex($"\\#{Field1Name} = :{Field1Name}_.{{32}}");

        config.ExpressionAttributeNames.Should().NotBeNull();
        config.ExpressionAttributeNames.Count.Should().Be(1);
        config.ExpressionAttributeNames.Keys.Should().Contain($"#{Field1Name}");
        config.ExpressionAttributeNames.Values.Should().Contain($"{Field1Name}");

        config.ExpressionAttributeValues.Should().NotBeNull();
        config.ExpressionAttributeValues.Count.Should().Be(1);
        config.ExpressionAttributeValues.Keys.First().Should().MatchRegex($":{Field1Name}_.{{32}}");
        config.ExpressionAttributeValues.Values.First().Should().BeEquivalentTo(new AttributeValue { S = Field1Value });
    }

    [Fact]
    public void AddCriteria_WithConditionTrue()
    {
        var criteria = new EqualDynamoCriteria<string>(Field1Name, Field1Value);
        var builder = AtomicBuilder
            .Create()
            .AddCriteria(true, () => criteria);

        var config = builder.Build();
        config.ConditionExpression.Should().MatchRegex($"\\#{Field1Name} = :{Field1Name}_.{{32}}");

        config.ExpressionAttributeNames.Should().NotBeNull();
        config.ExpressionAttributeNames.Count.Should().Be(1);
        config.ExpressionAttributeNames.Keys.Should().Contain($"#{Field1Name}");
        config.ExpressionAttributeNames.Values.Should().Contain($"{Field1Name}");

        config.ExpressionAttributeValues.Should().NotBeNull();
        config.ExpressionAttributeValues.Count.Should().Be(1);
        config.ExpressionAttributeValues.Keys.First().Should().MatchRegex($":{Field1Name}_.{{32}}");
        config.ExpressionAttributeValues.Values.First().Should().BeEquivalentTo(new AttributeValue { S = Field1Value });
    }

    [Fact]
    public void AddCriteria_WithConditionFalse()
    {
        var criteria = new EqualDynamoCriteria<string>(Field1Name, Field1Value);
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
            .SetKey(Field1Name, Field1Value)
            .SetKey(Field2Name, Field2Value);

        var config = builder.Build();

        config.Should().NotBeNull();

        var expectedConditions = new Dictionary<string, AttributeValue>
        {
            { Field1Name, new() { S = Field1Value } },
            { Field2Name, new() { N = Field2Value.ToString() } }
        };

        config.Key.Should().BeEquivalentTo(expectedConditions);
    }

    [Fact]
    public void UseParenthesis()
    {
        var left = new EqualDynamoCriteria<string>(Field1Name, Field1Value);
        var right = new EqualDynamoCriteria<long>(Field2Name, Field2Value);
        var criteria = new AndDynamoCriteria(left, right);

        var builder = AtomicBuilder
            .Create()
            .AddCriteria(criteria)
            .UseParenthesis();

        var config = builder.Build();
        config.ConditionExpression.Should().MatchRegex(
            $"\\(\\#{Field1Name} = :{Field1Name}_.{{32}} AND #{Field2Name} = :{Field2Name}_.{{32}}\\)");
    }

    [Fact]
    public void SetTableName_WhenConfigNotEmpty()
    {
        var dynamoConfig = new DynamoDBContextConfig
        {
            TableNamePrefix = "22cans_"
        };
        var builder = AtomicBuilder
            .Create()
            .SetTableName<DynaMightTestClass>(dynamoConfig);

        var config = builder.Build();
        config.TableName.Should().Be("22cans_DynaMightTestClass");
    }

    [Fact]
    public void SetTableName_WhenConfigEmpty()
    {
        var dynamoConfig = new DynamoDBContextConfig();
        var builder = AtomicBuilder
            .Create()
            .SetTableName<DynaMightTestClass>(dynamoConfig);

        var config = builder.Build();
        config.TableName.Should().Be("DynaMightTestClass");
    }

    [Fact]
    public void Operation_WhenConditionTrue()
    {
        var builder = AtomicBuilder
            .Create()
            .AddOperation(true, () => new AddAtomicOperation<long>(Field2Name, Field2Value));

        var config = builder.Build();

        config.UpdateExpression.Should().Be($"ADD #{Field2Name} :{Field2Name}");

        config.ExpressionAttributeNames.Should().BeEquivalentTo(new Dictionary<string, string>
        {
            { $"#{Field2Name}", Field2Name }
        });
        config.ExpressionAttributeValues.Should().BeEquivalentTo(new Dictionary<string, AttributeValue>
        {
            { $":{Field2Name}", new AttributeValue { N = Field2Value.ToString() } }
        });
    }

    [Fact]
    public void Operation_WhenConditionFalse()
    {
        var builder = AtomicBuilder
            .Create()
            .AddOperation(false, () => new AddAtomicOperation<long>(Field2Name, Field2Value));

        var config = builder.Build();

        config.UpdateExpression.Should().BeNullOrEmpty();

        config.ExpressionAttributeNames.Should().BeNullOrEmpty();
        config.ExpressionAttributeValues.Should().BeNullOrEmpty();
    }

    [Fact]
    public void OneOperation()
    {
        var builder = AtomicBuilder
            .Create()
            .AddOperation(new AddAtomicOperation<long>(Field2Name, Field2Value));

        var config = builder.Build();

        config.UpdateExpression.Should().Be($"ADD #{Field2Name} :{Field2Name}");

        config.ExpressionAttributeNames.Should().BeEquivalentTo(new Dictionary<string, string>
        {
            { $"#{Field2Name}", Field2Name }
        });
        config.ExpressionAttributeValues.Should().BeEquivalentTo(new Dictionary<string, AttributeValue>
        {
            { $":{Field2Name}", new AttributeValue { N = Field2Value.ToString() } }
        });
    }

    [Fact]
    public void TwoOperationsSameKind()
    {
        var builder = AtomicBuilder
            .Create()
            .AddOperation(new AddAtomicOperation<long>(Field2Name, Field2Value))
            .AddOperation(new AddAtomicOperation<long>(Field3Name, Field3Value));

        var config = builder.Build();

        config.UpdateExpression.Should().Be($"ADD #{Field2Name} :{Field2Name},#{Field3Name} :{Field3Name}");

        config.ExpressionAttributeNames.Should().BeEquivalentTo(new Dictionary<string, string>
        {
            { $"#{Field2Name}", Field2Name },
            { $"#{Field3Name}", Field3Name }
        });
        config.ExpressionAttributeValues.Should().BeEquivalentTo(new Dictionary<string, AttributeValue>
        {
            { $":{Field2Name}", new AttributeValue { N = Field2Value.ToString() } },
            { $":{Field3Name}", new AttributeValue { N = Field3Value.ToString() } }
        });
    }

    [Fact]
    public void TwoOperationsDifferentKind()
    {
        var builder = AtomicBuilder
            .Create()
            .AddOperation(new AddAtomicOperation<long>(Field2Name, Field2Value))
            .AddOperation(new SetAtomicOperation<int>(Field3Name, Field3Value));

        var config = builder.Build();

        config.UpdateExpression.Should().Be($"ADD #{Field2Name} :{Field2Name} SET #{Field3Name} = :{Field3Name}");

        config.ExpressionAttributeNames.Should().BeEquivalentTo(new Dictionary<string, string>
        {
            { $"#{Field2Name}", Field2Name },
            { $"#{Field3Name}", Field3Name }
        });
        config.ExpressionAttributeValues.Should().BeEquivalentTo(new Dictionary<string, AttributeValue>
        {
            { $":{Field2Name}", new AttributeValue { N = Field2Value.ToString() } },
            { $":{Field3Name}", new AttributeValue { N = Field3Value.ToString() } }
        });
    }

    [Fact]
    public void AddTwoOperationsForSameField_KeepsTheFirst()
    {
        var builder = AtomicBuilder
            .Create()
            .AddOperation(new SetAtomicOperation<long>(Field2Name, Field2Value))
            .AddOperation(new SetAtomicOperation<long>(Field2Name, 0));

        var config = builder.Build();
        config.UpdateExpression.Should().Be($"SET #{Field2Name} = :{Field2Name}");

        config.ExpressionAttributeNames.Should().BeEquivalentTo(new Dictionary<string, string>
        {
            { $"#{Field2Name}", Field2Name }
        });
        config.ExpressionAttributeValues.Should().BeEquivalentTo(new Dictionary<string, AttributeValue>
        {
            { $":{Field2Name}", new AttributeValue { N = Field2Value.ToString() } }
        });
    }

    [Fact]
    public void AddTwoOperationsForSameField_KeepsTheFirst_AndCriteriaBefore()
    {
        var builder = AtomicBuilder
            .Create()
            .AddCriteria(new NotExistsDynamoCriteria(Field2Name))
            .AddOperation(new SetAtomicOperation<long>(Field2Name, Field2Value))
            .AddOperation(new SetAtomicOperation<long>(Field2Name, 0));

        var config = builder.Build();
        config.UpdateExpression.Should().Be($"SET #{Field2Name} = :{Field2Name}");

        config.ExpressionAttributeNames.Should().BeEquivalentTo(new Dictionary<string, string>
        {
            { $"#{Field2Name}", Field2Name }
        });
        config.ExpressionAttributeValues.Should().BeEquivalentTo(new Dictionary<string, AttributeValue>
        {
            { $":{Field2Name}", new AttributeValue { N = Field2Value.ToString() } }
        });
    }

    [Fact]
    public void AddOperationWithEmptyKeyField_DontAdd()
    {
        var builder = AtomicBuilder
            .Create()
            .AddOperation(new SetAtomicOperation<long>(Field2Name, Field2Value))
            .AddOperation(new SetAtomicOperation<long>(string.Empty, 0));

        var config = builder.Build();
        config.UpdateExpression.Should().Be($"SET #{Field2Name} = :{Field2Name}");

        config.ExpressionAttributeNames.Should().BeEquivalentTo(new Dictionary<string, string>
        {
            { $"#{Field2Name}", Field2Name }
        });
        config.ExpressionAttributeValues.Should().BeEquivalentTo(new Dictionary<string, AttributeValue>
        {
            { $":{Field2Name}", new AttributeValue { N = Field2Value.ToString() } }
        });
    }

    [Fact]
    public void AddReferenceOperation()
    {
        var builder = AtomicBuilder
            .Create()
            .AddOperation(new ReferenceOperation(Field1Name));

        var config = builder.Build();
        config.UpdateExpression.Should().BeEmpty();

        config.ExpressionAttributeNames.Should().BeEquivalentTo(new Dictionary<string, string>
        {
            { $"#{Field1Name}", Field1Name }
        });
        config.ExpressionAttributeValues.Should().BeEmpty();
    }

    [Fact]
    public void AddCustomOperationWithReference()
    {
        var builder = AtomicBuilder
            .Create()
            .AddOperation(new CustomOperation(
                () => "SET",
                () => $"#{Field1Name} = :{Field1Name} + #{Field2Name}",
                () => ($"#{Field1Name}", Field1Name),
                () => ($":{Field1Name}", DynamoValueConverter.ToAttributeValue(Field1Value),
                    DynamoValueConverter.ToDynamoDbEntry(Field1Value))
            ))
            .AddOperation(new ReferenceOperation(Field2Name));

        var config = builder.Build();
        config.UpdateExpression.Should().Be($"SET #{Field1Name} = :{Field1Name} + #{Field2Name}");

        config.ExpressionAttributeNames.Should().BeEquivalentTo(new Dictionary<string, string>
        {
            { $"#{Field1Name}", Field1Name },
            { $"#{Field2Name}", Field2Name }
        });
        config.ExpressionAttributeValues.Should().BeEquivalentTo(new Dictionary<string, AttributeValue>
        {
            { $":{Field1Name}", new AttributeValue { S = Field1Value } }
        });
    }
}
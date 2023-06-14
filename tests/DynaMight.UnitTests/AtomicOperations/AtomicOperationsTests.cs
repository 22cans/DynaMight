using Amazon.DynamoDBv2.Model;
using DynaMight.AtomicOperations;
using FluentAssertions;

namespace DynaMight.UnitTests.AtomicOperations;

public class AtomicOperationsTests
{
    [Fact]
    public void Add()
    {
        var atomicOperation = new AddAtomicOperation<int>("field", 1);

        atomicOperation.Should().NotBeNull();
        atomicOperation.UpdateExpressionType.Should().Be("ADD");
        atomicOperation.GetUpdateExpression().Should().Be("#field :field");

        var (key, value) = atomicOperation.GetNameExpression();
        key.Should().Be("#field");
        value.Should().Be("field");

        var (keyName, attributeValue, dynamoDbEntry) = atomicOperation.GetValueExpression();
        keyName.Should().Be(":field");
        attributeValue.Should().BeEquivalentTo(new AttributeValue { N = "1" });
        dynamoDbEntry!.AsInt().Should().Be(1);
    }
    
    [Fact]
    public void Increment()
    {
        const int increment = 1;
        var atomicOperation = new IncrementAtomicOperation("field");

        var (field, attributeValue, dynamoDbEntry) = atomicOperation.GetValueExpression();

        atomicOperation.Should().NotBeNull();
        atomicOperation.UpdateExpressionType.Should().Be("SET");
        atomicOperation.GetUpdateExpression().Should().Be("#field = #field + :fieldIncrement");

        field.Should().Be(":fieldIncrement");
        attributeValue.Should().BeEquivalentTo(new AttributeValue { N = increment.ToString() });
        dynamoDbEntry!.AsInt().Should().Be(increment);
    }

    [Fact]
    public void IncrementBy()
    {
        const int increment = 13;
        var atomicOperation = new IncrementByAtomicOperation<int>("field", increment);

        var (field, attributeValue, dynamoDbEntry) = atomicOperation.GetValueExpression();

        atomicOperation.Should().NotBeNull();
        atomicOperation.UpdateExpressionType.Should().Be("SET");
        atomicOperation.GetUpdateExpression().Should().Be("#field = #field + :fieldIncrement");

        field.Should().Be(":fieldIncrement");
        attributeValue.Should().BeEquivalentTo(new AttributeValue { N = increment.ToString() });
        dynamoDbEntry!.AsInt().Should().Be(increment);
    }

    [Fact]
    public void Remove()
    {
        var atomicOperation = new RemoveFieldOperation("field");
        
        atomicOperation.Should().NotBeNull();
        atomicOperation.UpdateExpressionType.Should().Be("REMOVE");
        atomicOperation.GetUpdateExpression().Should().Be("#field");

        var (key, value) = atomicOperation.GetNameExpression();
        key.Should().Be("#field");
        value.Should().Be("field");

        var (keyName, attributeValue, dynamoDbEntry) = atomicOperation.GetValueExpression();
        keyName.Should().BeEmpty();
        attributeValue.Should().BeNull();
        dynamoDbEntry.Should().BeNull();
    }
    
    [Fact]
    public void Set()
    {
        var addAtomicOperation = new SetAtomicOperation<int>("field", 1);

        addAtomicOperation.Should().NotBeNull();
        addAtomicOperation.UpdateExpressionType.Should().Be("SET");
        addAtomicOperation.GetUpdateExpression().Should().Be("#field = :field");
    }
    
    [Fact]
    public void SetNonExistent()
    {
        var addAtomicOperation = new SetNonExistentAtomicOperation<int>("field", 1);

        addAtomicOperation.Should().NotBeNull();
        addAtomicOperation.UpdateExpressionType.Should().Be("SET");
        addAtomicOperation.GetUpdateExpression().Should().Be("#field = if_not_exists(#field, :field)");
    }
}
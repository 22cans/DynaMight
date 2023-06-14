using Amazon.DynamoDBv2.Model;
using DynaMight.Extensions;
using FluentAssertions;

namespace DynaMight.UnitTests.Extensions;

public class DictionaryExtensionsTests
{
    [Fact]
    public void Get_WhenDictDoesNotHaveKey()
    {
        var dictionary = new Dictionary<string, AttributeValue>();

        var result = dictionary.Get("notExists", value => value.S);

        result.Should().BeNull();
    }

    [Fact]
    public void GetInt_WhenDictDoesNotHaveKey()
    {
        var dictionary = new Dictionary<string, AttributeValue>();

        var result = dictionary.GetInt("notExists");

        result.Should().BeNull();
    }
    
    [Fact]
    public void GetInt_WhenDictHasKey()
    {
        var dictionary = new Dictionary<string, AttributeValue>
        {
            {"contains", new AttributeValue{ N = "123"}}
        };

        var result = dictionary.GetInt("contains");

        result.Should().NotBeNull();
        result.Should().Be(123);
    }
    
    [Fact]
    public void Add()
    {
        // ReSharper disable once UseObjectOrCollectionInitializer
        var dictionary = new Dictionary<string, string>();
        
        dictionary.Add(("contains", "yeah!"));

        dictionary.Count.Should().Be(1);
        dictionary.Keys.Should().Contain("contains");
        dictionary["contains"].Should().Be("yeah!");
    }
}
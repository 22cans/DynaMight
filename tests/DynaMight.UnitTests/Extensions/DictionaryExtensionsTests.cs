using Amazon.DynamoDBv2.Model;
using DynaMight.Extensions;
using FluentAssertions;

namespace DynaMight.UnitTests.Extensions;

public class DictionaryExtensionsTests
{
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
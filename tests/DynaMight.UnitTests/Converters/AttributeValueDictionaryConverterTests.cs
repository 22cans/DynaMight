using Amazon.DynamoDBv2.Model;
using DynaMight.Converters;
using FluentAssertions;

namespace DynaMight.UnitTests.Converters;

public class AttributeValueDictionaryConverterTests
{
    [Fact]
    public void ConvertEnum()
    {
        AttributeValueDictionaryConverter.AddCustomConverter<DynaMightTestClass.TestEnum>((obj, prop, dict) =>
            prop.SetValue(obj, (DynaMightTestClass.TestEnum)int.Parse(dict[prop.Name].N)));

        var result = AttributeValueDictionaryConverter.ConvertFrom<DynaMightTestClass>(
            new Dictionary<string, AttributeValue>
            {
                { "StringValue", new AttributeValue { S = "test" } },
                { "TestEnumValue", new AttributeValue { N = "0" } }
            });

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(new DynaMightTestClass
            { StringValue = "test", TestEnumValue = DynaMightTestClass.TestEnum.Value1 });
    }
}
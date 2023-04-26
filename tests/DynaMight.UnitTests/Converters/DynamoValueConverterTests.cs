using Amazon.DynamoDBv2.Model;
using DynaMight.Converters;
using FluentAssertions;

namespace DynaMight.UnitTests.Converters;

public class DynamoValueConverterTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(0L)]
    [InlineData(0D)]
    //TODO: check decimal test
    //[InlineData(0M)]
    public void AttributeValue_WhenReceiveNumber<T>(T number) where T : notnull
    {
        var result = DynamoValueConverter.From(number);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(new AttributeValue { N = number.ToString() });
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void AttributeValue_WhenReceiveBool(bool value)
    {
        var result = DynamoValueConverter.From(value);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(new AttributeValue { BOOL = value });
    }

    [Theory]
    [InlineData("string1")]
    [InlineData("string2")]
    public void AttributeValue_WhenReceiveString(string value)
    {
        var result = DynamoValueConverter.From(value);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(new AttributeValue { S = value });
    }

    [Fact]
    public void AttributeValue_WhenReceiveListString()
    {
        var list = new List<string>() { "string1", "string2" };
        var result = DynamoValueConverter.From(list);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(new AttributeValue { SS = list });
    }

    [Fact]
    public void AttributeValue_WhenReceiveNull()
    {
        int? nullValue = null;
        var result = DynamoValueConverter.From(nullValue);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(new AttributeValue { NULL = true });
    }


    [Theory]
    [InlineData(DynaMightTestClass.TestEnum.Value0)]
    [InlineData(DynaMightTestClass.TestEnum.Value1)]
    public void AttributeValue_WhenReceiveCustomEnum(DynaMightTestClass.TestEnum value)
    {
        CustomDynamoValueConverter.AddCustomConverter<DynaMightTestClass.TestEnum>(
            r => new AttributeValue { N = ((int)r).ToString() },
            r => (int)r);

        var result = DynamoValueConverter.From(value);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(new AttributeValue { N = ((int)value).ToString() });
    }
}
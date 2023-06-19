using Amazon.DynamoDBv2.Model;
using DynaMight.Converters;
using FluentAssertions;

namespace DynaMight.UnitTests.Converters;

public class DynamoDbEntryTests
{
    [Fact]
    public void ToDynamoDbEntry_int()
    {
        const int value = int.MaxValue;
        var result = DynamoValueConverter.ToDynamoDbEntry(value);

        result.Should().NotBeNull();
        result!.AsInt().Should().Be(value);
    }

    [Fact]
    public void ToDynamoDbEntry_long()
    {
        const long value = long.MaxValue;
        var result = DynamoValueConverter.ToDynamoDbEntry(value);

        result.Should().NotBeNull();
        result!.AsLong().Should().Be(value);
    }

    [Fact]
    public void ToDynamoDbEntry_double()
    {
        const double value = double.MaxValue;
        var result = DynamoValueConverter.ToDynamoDbEntry(value);

        result.Should().NotBeNull();
        result!.AsDouble().Should().Be(value);
    }

    [Fact]
    public void ToDynamoDbEntry_decimal()
    {
        const decimal value = decimal.MaxValue;
        var result = DynamoValueConverter.ToDynamoDbEntry(value);

        result.Should().NotBeNull();
        result!.AsDecimal().Should().Be(value);
    }

    [Fact]
    public void ToDynamoDbEntry_bool()
    {
        const bool value = true;
        var result = DynamoValueConverter.ToDynamoDbEntry(value);

        result.Should().NotBeNull();
        result!.AsBoolean().Should().Be(value);
    }

    [Fact]
    public void ToDynamoDbEntry_null()
    {
        var result = DynamoValueConverter.ToDynamoDbEntry(null);

        result.Should().BeNull();
    }

    [Fact]
    public void ToDynamoDbEntry_list()
    {
        var value = new List<string> { "string1", "string2" };
        var result = DynamoValueConverter.ToDynamoDbEntry(value);

        result.Should().NotBeNull();
        result!.AsListOfString().Should().BeEquivalentTo(value);
    }

    [Fact]
    public void ToDynamoDbEntry_exception()
    {
        var value = new List<byte>();

        Assert.Throws<NotSupportedException>(() => DynamoValueConverter.ToDynamoDbEntry(value));
    }

}
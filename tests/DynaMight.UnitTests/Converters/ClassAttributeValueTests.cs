using Amazon.DynamoDBv2.Model;
using DynaMight.Converters;
using FluentAssertions;

namespace DynaMight.UnitTests.Converters;

public class ClassAttributeValueTests
{
    [Fact]
    public void FromClass()
    {
        DynaMightCustomConverter.LoadAndRegister();

        var value = new DynaMightTestClass
        {
            StringValue = "test", TestEnumValue = DynaMightTestClass.TestEnum.Value0, IntValue = 100,
            DecimalValue = 200, LongValue = 300, DoubleValue = 350, BoolValue = true,
            StringListValue = new List<string> { "a", "b", "c" },
            TestAttributeEnumValue = DynaMightTestClass.TestAttributeEnum.ValueWithAttribute2,
            InternalClassValue = new DynaMightTestClass.InternalClass { InternalString = "internal string value" }
        };

        var result = DynamoValueConverter.ToClassAttributeValue(value);

        result.Should().NotBeNullOrEmpty();

        result.Should().BeEquivalentTo(new Dictionary<string, AttributeValue>
        {
            { "StringValue", new AttributeValue { S = "test" } },
            { "StringNullableValue", new AttributeValue { NULL = true } },
            { "TestEnumValue", new AttributeValue { N = "0" } },
            { "TestEnumNullableValue", new AttributeValue { NULL = true } },
            { "IntValue", new AttributeValue { N = "100" } },
            { "IntNullableValue", new AttributeValue { NULL = true } },
            { "DecimalValue", new AttributeValue { N = "200" } },
            { "DecimalNullableValue", new AttributeValue { NULL = true } },
            { "LongValue", new AttributeValue { N = "300" } },
            { "LongNullableValue", new AttributeValue { NULL = true } },
            { "DoubleValue", new AttributeValue { N = "350" } },
            { "DoubleNullableValue", new AttributeValue { NULL = true } },
            { "BoolValue", new AttributeValue { BOOL = true } },
            { "BoolNullableValue", new AttributeValue { NULL = true } },
            { "StringListValue", new AttributeValue { SS = new List<string> { "a", "b", "c" } } },
            { "StringListNullableValue", new AttributeValue { NULL = true } },
            { "TestAttributeEnumValue", new AttributeValue { N = "2" } },
            { "TestAttributeEnumNullableValue", new AttributeValue { NULL = true } },
            {
                "InternalClassValue", new AttributeValue
                {
                    M = new Dictionary<string, AttributeValue>
                    {
                        { "InternalString", new AttributeValue { S = "internal string value" } }
                    }
                }
            },
            { "InternalClassNullableValue", new AttributeValue { NULL = true } },
            { "DateTimeValue", new AttributeValue { S = "2023-08-29T15:44:27" } },
            { "DateTimeNullableValue", new AttributeValue { NULL = true } },
        });
    }
}
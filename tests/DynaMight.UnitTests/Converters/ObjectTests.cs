using Amazon.DynamoDBv2.Model;
using DynaMight.Converters;
using FluentAssertions;

namespace DynaMight.UnitTests.Converters;

public class ObjectTests
{
    [Fact]
    public void ConvertToClassWithNulls()
    {
        DynaMightCustomConverter.LoadAndRegister();

        var result = DynamoValueConverter.To<DynaMightTestClass>(
            new Dictionary<string, AttributeValue>
            {
                { "StringValue", new AttributeValue { S = "test" } },
                { "TestEnumValue", new AttributeValue { N = "0" } },
                { "IntValue", new AttributeValue { N = "100" } },
                { "DecimalValue", new AttributeValue { N = "200" } },
                { "DoubleValue", new AttributeValue { N = "350" } },
                { "LongValue", new AttributeValue { N = "300" } },
                { "BoolValue", new AttributeValue { BOOL = true } },
                { "StringListValue", new AttributeValue { SS = new List<string> { "a", "b", "c" } } },
                { "TestAttributeEnumValue", new AttributeValue { N = "2" } },
                {
                    "InternalClassValue", new AttributeValue
                    {
                        M = new Dictionary<string, AttributeValue>
                        {
                            { "InternalString", new AttributeValue { S = "internal string value" } }
                        }
                    }
                }
            });

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(new DynaMightTestClass
        {
            StringValue = "test", TestEnumValue = DynaMightTestClass.TestEnum.Value0, IntValue = 100,
            DecimalValue = 200, LongValue = 300, DoubleValue = 350, BoolValue = true,
            StringListValue = new List<string> { "a", "b", "c" },
            TestAttributeEnumValue = DynaMightTestClass.TestAttributeEnum.ValueWithAttribute2,
            InternalClassValue = new DynaMightTestClass.InternalClass { InternalString = "internal string value" }
        });
    }

    [Fact]
    public void ConvertToClassWithFilledNulls()
    {
        DynaMightCustomConverter.LoadAndRegister();

        var result = DynamoValueConverter.To<DynaMightTestClass>(
            new Dictionary<string, AttributeValue>
            {
                { "StringValue", new AttributeValue { S = "test" } },
                { "TestEnumValue", new AttributeValue { N = "0" } },
                { "IntValue", new AttributeValue { N = "100" } },
                { "DecimalValue", new AttributeValue { N = "200" } },
                { "DoubleValue", new AttributeValue { N = "350" } },
                { "LongValue", new AttributeValue { N = "300" } },
                { "BoolValue", new AttributeValue { BOOL = true } },
                { "IntNullableValue", new AttributeValue { N = "150" } },
                { "DecimalNullableValue", new AttributeValue { N = "250" } },
                { "DoubleNullableValue", new AttributeValue { N = "350" } },
                { "LongNullableValue", new AttributeValue { N = "350" } },
                { "BoolNullableValue", new AttributeValue { BOOL = false } },
                { "StringListValue", new AttributeValue { SS = new List<string> { "a", "b", "c" } } },
                { "StringListNullableValue", new AttributeValue { SS = new List<string> { "d", "e", "f" } } },
                { "TestEnumNullableValue", new AttributeValue { N = "1" } },
                { "TestAttributeEnumValue", new AttributeValue { N = "2" } },
                { "TestAttributeEnumNullableValue", new AttributeValue { N = "1" } },
                {
                    "InternalClassValue", new AttributeValue
                    {
                        M = new Dictionary<string, AttributeValue>
                        {
                            { "InternalString", new AttributeValue { S = "internal string value" } }
                        }
                    }
                },
                {
                    "InternalClassNullableValue", new AttributeValue
                    {
                        M = new Dictionary<string, AttributeValue>
                        {
                            { "InternalString", new AttributeValue { S = "another string value" } }
                        }
                    }
                }
            });

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(new DynaMightTestClass
        {
            StringValue = "test", TestEnumValue = DynaMightTestClass.TestEnum.Value0, IntValue = 100,
            DecimalValue = 200, LongValue = 300, DoubleValue = 350, BoolValue = true, IntNullableValue = 150, DecimalNullableValue = 250,
            LongNullableValue = 350, DoubleNullableValue = 350, BoolNullableValue = false,
            StringListValue = new List<string> { "a", "b", "c" },
            StringListNullableValue = new List<string> { "d", "e", "f" },
            TestEnumNullableValue = DynaMightTestClass.TestEnum.Value1,
            TestAttributeEnumValue = DynaMightTestClass.TestAttributeEnum.ValueWithAttribute2,
            TestAttributeEnumNullableValue = DynaMightTestClass.TestAttributeEnum.ValueWithAttribute1,
            InternalClassValue = new DynaMightTestClass.InternalClass { InternalString = "internal string value" },
            InternalClassNullableValue = new DynaMightTestClass.InternalClass
                { InternalString = "another string value" }
        });
    }

    [Fact]
    public void ConvertWithNullableError()
    {
        DynaMightCustomConverter.LoadAndRegister();

        Assert.Throws<KeyNotFoundException>(() =>
            DynamoValueConverter.To<DynaMightTestClass>(
                new Dictionary<string, AttributeValue>
                {
                    // Missing this key
                    //{ "StringValue", new AttributeValue { S = "test" } },
                    { "TestEnumValue", new AttributeValue { N = "0" } },
                    { "IntValue", new AttributeValue { N = "100" } },
                    { "DecimalValue", new AttributeValue { N = "200" } },
                    { "LongValue", new AttributeValue { N = "300" } },
                    { "BoolValue", new AttributeValue { BOOL = true } },
                    { "TestAttributeEnumValue", new AttributeValue { N = "2" } },
                    {
                        "InternalClassValue", new AttributeValue
                        {
                            M = new Dictionary<string, AttributeValue>
                            {
                                { "InternalString", new AttributeValue { S = "internal string value" } }
                            }
                        }
                    }
                })
        );
    }

    [Fact]
    public void UnsupportedType()
    {
        DynaMightCustomConverter.LoadAndRegister();
        Assert.Throws<NotSupportedException>(() => DynamoValueConverter.To<UnsupportedTypeClass>(
            new Dictionary<string, AttributeValue>
            {
                {
                    "CharValue", new AttributeValue { S = "test" }
                },
            }));
    }
}
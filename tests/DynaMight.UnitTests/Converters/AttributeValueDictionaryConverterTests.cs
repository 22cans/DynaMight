using Amazon.DynamoDBv2.Model;
using DynaMight.Converters;
using DynaMight.Extensions;
using FluentAssertions;

namespace DynaMight.UnitTests.Converters;

public class AttributeValueDictionaryConverterTests
{
    [Fact]
    public void ConvertClassWithNulls()
    {
        // DynaMightCustomConverter.RegisterEnum<DynaMightTestClass.TestEnum>();
        // DynaMightCustomConverter.RegisterEnum<DynaMightTestClass.TestAttributeEnum>();

        DynaMightCustomConverter.LoadAndRegister();

        var result = AttributeValueDictionaryConverter.ConvertFrom<DynaMightTestClass>(
            new Dictionary<string, AttributeValue>
            {
                { "StringValue", new AttributeValue { S = "test" } },
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
            });

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(new DynaMightTestClass
        {
            StringValue = "test", TestEnumValue = DynaMightTestClass.TestEnum.Value0, IntValue = 100,
            DecimalValue = 200, LongValue = 300, BoolValue = true,
            TestAttributeEnumValue = DynaMightTestClass.TestAttributeEnum.ValueWithAttribute2,
            InternalClassValue = new DynaMightTestClass.InternalClass { InternalString = "internal string value" }
        });
    }

    [Fact]
    public void ConvertClassWithFilledNulls()
    {
        AttributeValueDictionaryConverter.AddCustomConverter<DynaMightTestClass.TestEnum>((obj, prop, dict) =>
            prop.SetValue(obj, (DynaMightTestClass.TestEnum)int.Parse(dict[prop.Name].N)));
        AttributeValueDictionaryConverter.AddCustomConverter<DynaMightTestClass.TestEnum?>((obj, prop, dict) =>
            prop.SetValue(obj,
                dict.Get<DynaMightTestClass.TestEnum?>(prop.Name, x => (DynaMightTestClass.TestEnum)int.Parse(x.N))));

        var result = AttributeValueDictionaryConverter.ConvertFrom<DynaMightTestClass>(
            new Dictionary<string, AttributeValue>
            {
                { "StringValue", new AttributeValue { S = "test" } },
                { "TestEnumValue", new AttributeValue { N = "0" } },
                { "IntValue", new AttributeValue { N = "100" } },
                { "DecimalValue", new AttributeValue { N = "200" } },
                { "LongValue", new AttributeValue { N = "300" } },
                { "BoolValue", new AttributeValue { BOOL = true } },
                { "IntNullableValue", new AttributeValue { N = "150" } },
                { "DecimalNullableValue", new AttributeValue { N = "250" } },
                { "LongNullableValue", new AttributeValue { N = "350" } },
                { "BoolNullableValue", new AttributeValue { BOOL = false } },
                { "TestEnumNullableValue", new AttributeValue { N = "1" } },
                { "TestAttributeEnumValue", new AttributeValue { N = "2" } },
                { "TestAttributeNullableEnumValue", new AttributeValue { N = "1" } },
            });

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(new DynaMightTestClass
        {
            StringValue = "test", TestEnumValue = DynaMightTestClass.TestEnum.Value0, IntValue = 100,
            DecimalValue = 200, LongValue = 300, BoolValue = true, IntNullableValue = 150, DecimalNullableValue = 250,
            LongNullableValue = 350, BoolNullableValue = false,
            TestEnumNullableValue = DynaMightTestClass.TestEnum.Value1,
            TestAttributeEnumValue = DynaMightTestClass.TestAttributeEnum.ValueWithAttribute2,
            TestAttributeEnumNullableValue = DynaMightTestClass.TestAttributeEnum.ValueWithAttribute1
        });
    }
}
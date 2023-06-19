using Amazon.DynamoDBv2.DataModel;
using DynaMight.Converters;

namespace DynaMight.UnitTests;

[DynamoDBTable("DynaMightTestClass")]
public class DynaMightTestClass
{
    public string StringValue { get; set; } = default!;
    public string? StringNullableValue { get; set; }
    public TestEnum TestEnumValue { get; set; }
    public TestEnum? TestEnumNullableValue { get; set; }
    public TestAttributeEnum TestAttributeEnumValue { get; set; }
    public TestAttributeEnum? TestAttributeEnumNullableValue { get; set; }
    public int IntValue { get; set; }
    public int? IntNullableValue { get; set; }
    public decimal DecimalValue { get; set; }
    public decimal? DecimalNullableValue { get; set; }
    public double DoubleValue { get; set; }
    public double? DoubleNullableValue { get; set; }
    public long LongValue { get; set; }
    public long? LongNullableValue { get; set; }
    public bool BoolValue { get; set; }
    public bool? BoolNullableValue { get; set; }
    public List<string> StringListValue { get; set; } = default!;
    public List<string>? StringListNullableValue { get; set; }
    public InternalClass InternalClassValue { get; set; } = default!;
    public InternalClass? InternalClassNullableValue { get; set; }

    [DynamoDBIgnore] public InternalClassNotSerializable? IgnoreClass { get; set; }

    [DynaMightCustomConverter]
    public enum TestEnum
    {
        Value0,
        Value1,
        Value2,
    }

    [DynaMightCustomConverter]
    public enum TestAttributeEnum
    {
        ValueWithAttribute0,
        ValueWithAttribute1,
        ValueWithAttribute2,
    }

    [DynaMightCustomConverter]
    public class InternalClass
    {
        public string InternalString { get; set; } = default!;
    }

    public class InternalClassNotSerializable
    {
        public string InternalString { get; set; } = default!;
    }
}

[DynaMightCustomConverter]
public class UnsupportedTypeClass
{
    public char CharValue { get; set; }
}
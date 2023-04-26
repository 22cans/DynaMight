using DynaMight.Converters;

namespace DynaMight.UnitTests;

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
    public long LongValue { get; set; }
    public long? LongNullableValue { get; set; }
    public bool BoolValue { get; set; }
    public bool? BoolNullableValue { get; set; }

    // public InternalClass InternalClassValue { get; set; } = default!;
    // public InternalClass? InternalClassNullableValue { get; set; }
    
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

    public class InternalClass
    {
        public string InternalString { get; set; }
    }
}
namespace DynaMight.UnitTests;

public class DynaMightTestClass
{
    public string StringValue { get; set; } = default!;
    public string? StringNullableValue { get; set; }
    public TestEnum TestEnumValue { get; set; }
    public TestEnum? TestEnumNullableValue { get; set; }
    public int IntValue { get; set; }
    public int? IntNullableValue { get; set; }
    public decimal DecimalValue { get; set; }
    public decimal? DecimalNullableValue { get; set; }
    public long LongValue { get; set; }
    public long? LongNullableValue { get; set; }
    public bool BoolValue { get; set; }
    public bool? BoolNullableValue { get; set; }
    
    public enum TestEnum
    {
        Value0,
        Value1,
    }
}
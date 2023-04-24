namespace DynaMight.UnitTests;

public class DynaMightTestClass
{
    public string StringValue { get; set; } = default!;
    public TestEnum TestEnumValue { get; set; }
    
    public enum TestEnum
    {
        Value1,
        Value2,
    }
}
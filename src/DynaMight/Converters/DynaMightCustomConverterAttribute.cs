namespace DynaMight.Converters;

/// <summary>
/// This item will be registered by the <see cref="DynaMightCustomConverter.LoadAndRegister"/> function.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum)]
public class DynaMightCustomConverterAttribute : Attribute
{
}
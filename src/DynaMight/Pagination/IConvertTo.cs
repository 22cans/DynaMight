namespace DynaMight.Pagination;

#pragma warning disable CA2252
public interface IConvertFrom<in TX, out TY>
{
    static abstract TY ConvertFrom(TX original);
}
#pragma warning restore CA2252
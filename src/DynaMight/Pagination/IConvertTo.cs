namespace DynaMight.Pagination;

#pragma warning disable CA2252
/// <summary>
/// Interface to specify the conversion method from the type TX to type TY, both generics.
/// You will need to implement the method ConvertFrom.
/// </summary>
/// <typeparam name="TX"></typeparam>
/// <typeparam name="TY"></typeparam>
public interface IConvertFrom<in TX, out TY>
{
    /// <summary>
    /// Converts the page of type <typeparamref name="TX"/> to a page of type <typeparamref name="TY"/>  
    /// </summary>
    /// <returns>A new page of type <typeparamref name="TY"/></returns>
    static abstract TY ConvertFrom(TX original);
}
#pragma warning restore CA2252
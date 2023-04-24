namespace DynaMight.Pagination;

public sealed record Page<T>(string PageToken, IList<T>? Results)
{
    public Page<TY> ConvertTo<TY>() where TY : IConvertFrom<T, TY>
    {
        var response = new Page<TY>(PageToken, Results?.Select(TY.ConvertFrom).ToList());
        return response;
    }

    public static Page<T> Empty() => new(string.Empty, Array.Empty<T>());
}
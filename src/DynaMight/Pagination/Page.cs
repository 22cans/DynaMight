namespace DynaMight.Pagination;

/// <summary>
/// Paged list of Results of Type `T`.
/// </summary>
/// <param name="PageToken">The page token used by DynamoDB to do pagination</param>
/// <param name="Results">The list of results for the query</param>
/// <typeparam name="T">The mapped type from the DynamoDB to the C# class</typeparam>
public sealed record Page<T>(string PageToken, IList<T>? Results)
{
    public Page<TY> ConvertTo<TY>() where TY : IConvertFrom<T, TY>
    {
        var response = new Page<TY>(PageToken, Results?.Select(TY.ConvertFrom).ToList());
        return response;
    }

    public static Page<T> Empty() => new(string.Empty, Array.Empty<T>());
}
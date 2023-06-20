namespace DynaMight.Pagination;

/// <summary>
/// Paged list of Results of Type `T`.
/// </summary>
/// <param name="PageToken">The page token used by DynamoDB to do pagination</param>
/// <param name="Results">The list of results for the query</param>
/// <typeparam name="T">The mapped type from the DynamoDB to the C# class</typeparam>
public sealed record Page<T>(string PageToken, IList<T>? Results)
{
    /// <summary>
    /// Converts the page of type <typeparamref name="T"/> to a page of type <typeparamref name="TY"/>  
    /// </summary>
    /// <typeparam name="TY">The target type</typeparam>
    /// <returns>A new page of type <typeparamref name="TY"/></returns>
    public Page<TY> ConvertTo<TY>() where TY : IConvertFrom<T, TY>
    {
        var response = new Page<TY>(PageToken, Results?.Select(TY.ConvertFrom).ToList());
        return response;
    }

    /// <summary>
    /// Returns an empty page of type <typeparamref name="T"/>
    /// </summary>
    /// <returns>An empty page of type <typeparamref name="T"/></returns>
    public static Page<T> Empty() => new(string.Empty, Array.Empty<T>());
}
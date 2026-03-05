namespace CommonFramework;

public static class AsyncEnumerableExtensions
{
    public static async IAsyncEnumerable<T> ToAsyncEnumerable<T>(this ValueTask<T?> task)
    {
        var value = await task;

        if (value is not null)
        {
            yield return value;
        }
    }

    public static IAsyncEnumerable<T> GetAllElements<T>(this IAsyncEnumerable<T> source, Func<T, IAsyncEnumerable<T>> getChildFunc)
    {
        return source.SelectMany(child => child.GetAllElements(getChildFunc));
    }

    public static IAsyncEnumerable<T> GetAllElements<T>(this T? source, Func<T, ValueTask<T?>> getNextFunc, bool skipFirstElement)
        where T : class
    {
        var baseElements = source.GetAllElements(getNextFunc);

        return skipFirstElement ? baseElements.Skip(1) : baseElements;
    }

    public static async IAsyncEnumerable<T> GetAllElements<T>(this T? source, Func<T, ValueTask<T?>> getNextFunc)
        where T : class
    {
        for (var state = source; state != null; state = await getNextFunc(state))
        {
            yield return state;
        }
    }

    public static async IAsyncEnumerable<T> GetAllElements<T>(this T source, Func<T, IAsyncEnumerable<T>> getChildFunc)
    {
        yield return source;

        await foreach (var element in getChildFunc(source).SelectMany(child => child.GetAllElements(getChildFunc)))
        {
            yield return element;
        }
    }
}
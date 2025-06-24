namespace CommonFramework;

public static class ObjectExtensions
{
    public static IEnumerable<T> GetAllElements<T>(this T source, Func<T, IEnumerable<T>> getChildFunc)
    {
        yield return source;

        foreach (var element in getChildFunc(source).SelectMany(child => child.GetAllElements(getChildFunc)))
        {
            yield return element;
        }
    }
}
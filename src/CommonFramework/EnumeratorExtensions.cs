namespace CommonFramework;

public static class EnumeratorExtensions
{
    public static IEnumerable<T> ReadToEnd<T>(this IEnumerator<T> source)
    {
        while (source.MoveNext())
        {
            yield return source.Current;
        }
    }

    public static IEnumerable<T> ReadMany<T>(this IEnumerator<T> source, int count)
    {
        return Enumerable.Range(0, count).Select(_ => source.ReadSingle());
    }

    public static T ReadSingle<T>(this IEnumerator<T> source)
    {
        if (source.MoveNext())
        {
            return source.Current;
        }
        else
        {
            throw new Exception("enumerator finished");
        }
    }
}
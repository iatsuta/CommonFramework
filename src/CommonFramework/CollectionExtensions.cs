namespace CommonFramework;

public static class CollectionExtensions
{
    public static void AddRange<T>(this ICollection<T> source, IEnumerable<T> args)
    {
        args.Foreach(source.Add);
    }
}
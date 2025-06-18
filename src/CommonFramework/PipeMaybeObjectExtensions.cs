namespace CommonFramework;

public static class PipeMaybeObjectExtensions
{
    public static void Maybe<TSource>(this TSource? source, Action<TSource> evaluate)
        where TSource : class
    {
        if (null != source)
        {
            evaluate(source);
        }
    }

    public static TResult? Maybe<TSource, TResult>(this TSource? source, Func<TSource, TResult> selector)
    {
        return null == source ? default : selector(source);
    }
}
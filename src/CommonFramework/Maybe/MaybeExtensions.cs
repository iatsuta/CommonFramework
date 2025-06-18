namespace CommonFramework;

public static class MaybeExtensions
{
    public static Maybe<TResult> Select<TSource, TResult>(this Maybe<TSource> source, Func<TSource, TResult> selector)
    {
        return source.Match(result => Maybe.Return(selector(result)), () => Maybe<TResult>.Nothing);
    }

    public static Maybe<TResult> SelectMany<TSource, TNextResult, TResult>(this Maybe<TSource> source, Func<TSource, Maybe<TNextResult>> nextSelector,
        Func<TSource, TNextResult, TResult> resultSelector)
    {
        return source.Match(result1 => nextSelector(result1).Match(result2 => Maybe.Return(resultSelector(result1, result2)),
                () => Maybe<TResult>.Nothing),
            () => Maybe<TResult>.Nothing);
    }


    public static TResult Match<TSource, TResult>(this Maybe<TSource> maybeValue, Func<TSource, TResult> fromJustResult, Func<TResult> fromNothingResult)
    {
        return maybeValue switch
        {
            Just<TSource> just => fromJustResult(just.Value),
            _ => fromNothingResult()
        };
    }

    public static Maybe<TResult> Or<TSource, TResult>(this Maybe<TSource> v1, Func<Maybe<TResult>> getV2)
        where TSource : TResult
    {
        return v1.HasValue ? v1.Select(v => (TResult)v) : getV2();
    }

    public static T GetValue<T>(this Maybe<T> maybeValue)
    {
        return maybeValue.GetValue(() => new Exception("Nothing Value"));
    }

    public static T GetValue<T>(this Maybe<T> maybeValue, Func<Exception> nothingException)
    {
        return maybeValue.Match(result => result, () => throw nothingException());
    }
}
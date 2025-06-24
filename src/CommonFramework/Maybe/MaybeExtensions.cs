namespace CommonFramework.Maybe;

public static class MaybeExtensions
{
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

    public static T? GetValueOrDefault<T>(this Maybe<T> maybeValue)
    {
        return maybeValue.GetValueOrDefault(default(T));
    }

    public static TResult GetValueOrDefault<TSource, TResult>(this Maybe<TSource> maybeValue, TResult defaultValue)
        where TSource : TResult
    {
        return maybeValue.Select(v => (TResult)v).GetValueOrDefault(() => defaultValue);
    }

    public static TResult GetValueOrDefault<TSource, TResult>(this Maybe<TSource> maybeValue, Func<TResult> getDefaultValue)
        where TSource : TResult
    {
        return maybeValue.Match(result => result, getDefaultValue);
    }

    public static Maybe<bool> LogicOr(this Maybe<bool> v1, Func<Maybe<bool>> getV2)
    {

        return v1.Where(p => p).Or(() => LogicOrDict[v1][getV2()]);
    }

    public static Maybe<bool> LogicOr(this Maybe<bool> v1, Maybe<bool> v2)
    {
        return v1.LogicOr(() => v2);
    }

    public static Maybe<bool> LogicAnd(this Maybe<bool> v1, Func<Maybe<bool>> getV2)
    {
        return v1.Where(p => !p).Or(() => LogicAndDict[v1][getV2()]);
    }

    private static readonly Dictionary<Maybe<bool>, Dictionary<Maybe<bool>, Maybe<bool>>> LogicOrDict = new Dictionary<Maybe<bool>, Dictionary<Maybe<bool>, Maybe<bool>>>()
    {
        //{ Maybe.Return(true), new Dictionary<Maybe<bool>, Maybe<bool>>
        //                      {
        //                          { Maybe.Return(true), Maybe.Return(true) },
        //                          { Maybe.Return(false), Maybe.Return(true) },
        //                          { Maybe<bool>.Nothing, Maybe.Return(true) }
        //                      } },

        { Maybe.Return(false), new Dictionary<Maybe<bool>, Maybe<bool>>
        {
            { Maybe.Return(true), Maybe.Return(true) },
            { Maybe.Return(false), Maybe.Return(false) },
            { Maybe<bool>.Nothing, Maybe<bool>.Nothing }
        } },

        {  Maybe<bool>.Nothing, new Dictionary<Maybe<bool>, Maybe<bool>>
        {
            { Maybe.Return(true), Maybe.Return(true) },
            { Maybe.Return(false), Maybe<bool>.Nothing },
            { Maybe<bool>.Nothing, Maybe<bool>.Nothing }
        } },
    };


    private static readonly Dictionary<Maybe<bool>, Dictionary<Maybe<bool>, Maybe<bool>>> LogicAndDict = new Dictionary<Maybe<bool>, Dictionary<Maybe<bool>, Maybe<bool>>>()
    {
        { Maybe.Return(true), new Dictionary<Maybe<bool>, Maybe<bool>>
        {
            { Maybe.Return(true), Maybe.Return(true) },
            { Maybe.Return(false), Maybe.Return(false) },
            { Maybe<bool>.Nothing, Maybe<bool>.Nothing }
        } },

        //{ Maybe.Return(false), new Dictionary<Maybe<bool>, Maybe<bool>>
        //                       {
        //                           { Maybe.Return(true), Maybe.Return(false) },
        //                           { Maybe.Return(false), Maybe.Return(false) },
        //                           { Maybe<bool>.Nothing, Maybe.Return(false) }
        //                       } },

        {  Maybe<bool>.Nothing, new Dictionary<Maybe<bool>, Maybe<bool>>
        {
            { Maybe.Return(true), Maybe<bool>.Nothing },
            { Maybe.Return(false), Maybe.Return(false) },
            { Maybe<bool>.Nothing, Maybe<bool>.Nothing }
        } },
    };
}
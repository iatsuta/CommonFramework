namespace CommonFramework.Maybe;

public abstract record Maybe<T>
{
    public abstract bool HasValue { get; }

    public static readonly Maybe<T> Nothing = new Nothing<T>();
}

public static class Maybe
{
    public static Maybe<Ignore> Return()
    {
        return Return(Ignore.Value);
    }

    public static Maybe<T> ToMaybe<T>(this T? value)
        where T : struct
    {
        return OfCondition(value != null, () => value!.Value);
    }

    public static Maybe<T> Return<T>(T value)
    {
        return new Just<T>(value);
    }

    public static Maybe<T> ToMaybe<T>(this T? value)
        where T : class
    {
        return OfCondition(value != null, () => value!);
    }

    public static Maybe<T> OfCondition<T>(bool condition, Func<T> getJustValue)
    {
        return OfCondition(condition, () => Return(getJustValue()), () => Maybe<T>.Nothing);
    }

    public static Maybe<T> OfCondition<T>(bool condition, Func<Maybe<T>> getTrueValue, Func<Maybe<T>> getFalseValue)
    {
        return condition ? getTrueValue()
            : getFalseValue();
    }

    public static Func<TArg, Maybe<TResult>> OfTryMethod<TArg, TResult>(TryMethod<TArg, TResult> tryAction)
    {
        return arg =>
        {
            return OfCondition(tryAction(arg, out var result), () => result);
        };
    }

    public static Func<TArg1, TArg2, Maybe<TResult>> OfTryMethod<TArg1, TArg2, TResult>(TryMethod<TArg1, TArg2, TResult> tryAction)
    {
        return (arg1, arg2) =>
        {
            return OfCondition(tryAction(arg1, arg2, out var result), () => result);
        };
    }

    public delegate bool TryMethod<in TArg, TResult>(TArg arg, out TResult result);

    public delegate bool TryMethod<in TArg1, in TArg2, TResult>(TArg1 arg1, TArg2 arg2, out TResult result);
}    
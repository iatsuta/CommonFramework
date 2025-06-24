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
}    
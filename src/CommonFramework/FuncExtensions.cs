namespace CommonFramework;

public static class FuncExtensions
{
    public static Func<TResult> WithCache<TResult>(this Func<TResult> func)
    {
        var lazyValue = func.ToLazy();

        return () => lazyValue.Value;
    }

    public static Lazy<T> ToLazy<T>(this Func<T> getValueFunc)
    {
        return new Lazy<T>(getValueFunc);
    }
}
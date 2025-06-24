namespace CommonFramework;

public static class FuncHelper
{
    public static Func<T, TResult> Create<T, TResult>(Func<T, TResult> func)
    {
        return func;
    }
}
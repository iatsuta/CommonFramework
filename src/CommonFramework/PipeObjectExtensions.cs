namespace CommonFramework;

public static class PipeObjectExtensions
{
    public static TResult Pipe<TSource, TResult>(this TSource source, bool condition, Func<TSource, TResult> evaluate)
        where TSource : TResult
    {
        return condition ? evaluate(source) : source;
    }
}
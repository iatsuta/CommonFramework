namespace CommonFramework;

public static class StringExtensions
{
    public static string Skip(this string input, string pattern, bool raiseIfNotEquals)
    {
        return input.Skip(pattern, StringComparison.CurrentCulture, raiseIfNotEquals);
    }

    public static string Skip(this string input, string pattern, StringComparison stringComparison, bool raiseIfNotEquals)
    {
        if (input.StartsWith(pattern, stringComparison))
        {
            return input.Substring(pattern.Length, input.Length - pattern.Length);
        }
        else if (raiseIfNotEquals)
        {
            throw new ArgumentException($"Invalid input: {input}. Expected start element: {pattern}", nameof(input));
        }
        else
        {
            return input;
        }
    }

    public static string SkipLast(this string input, string pattern, bool raiseIfNotEquals)
    {
        return input.SkipLast(pattern, StringComparison.CurrentCulture, raiseIfNotEquals);
    }

    public static string SkipLast(this string input, string pattern, StringComparison stringComparison, bool raiseIfNotEquals)
    {
        if (input.EndsWith(pattern, stringComparison))
        {
            return input.Substring(0, input.Length - pattern.Length);
        }
        else if (raiseIfNotEquals)
        {
            throw new ArgumentException($"Invalid input: {input}. Expected last element: {pattern}", nameof(input));
        }
        else
        {
            return input;
        }
    }

    public static string Join<TSource, TResult>(this IEnumerable<TSource> source, string separator, Func<TSource, TResult> selector)
    {
        return source.Select(selector).Join(separator);
    }
    public static string Join<T>(this IEnumerable<T> source, string separator)
    {
        return string.Join(separator, source);
    }
}
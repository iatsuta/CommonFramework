using CommonFramework.Maybe;

namespace CommonFramework;

public static class EnumerableExtensions
{
    public static IEnumerable<T> SelectMany<T>(this IEnumerable<IEnumerable<T>> source)
    {
        return source.SelectMany(v => v);
    }

    public static TResult Match<TSource, TResult>(this IEnumerable<TSource> source, Func<TResult> emptyFunc, Func<TSource, TResult> singleFunc, Func<TSource[], TResult> manyFunc)
    {
        var cache = source.ToArray();

        return cache.Length switch
        {
            0 => emptyFunc(),
            1 => singleFunc(cache.Single()),
            _ => manyFunc(cache)
        };
    }

    public static bool IsIntersected<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource> other)
    {
        return source.IsIntersected(other, EqualityComparer<TSource>.Default);
    }

    public static bool IsIntersected<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource> other, IEqualityComparer<TSource> comparer)
    {
        return source.Intersect(other, comparer).Any();
    }

    public static IEnumerable<TResult> EmptyIfNull<TResult>(this IEnumerable<TResult>? source)
    {
        return source ?? [];
    }

    public static IEnumerable<T> GetAllElements<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> getChildFunc)
    {
        return source.SelectMany(child => child.GetAllElements(getChildFunc));
    }

    public static IEnumerable<T> GetAllElements<T>(this T? source, Func<T, T?> getNextFunc, bool skipFirstElement)
        where T : class
    {
        var baseElements = source.GetAllElements(getNextFunc);

        return skipFirstElement ? baseElements.Skip(1) : baseElements;
    }

    public static IEnumerable<T> GetAllElements<T>(this T? source, Func<T, T?> getNextFunc)
        where T : class
    {
        for (var state = source; state != null; state = getNextFunc(state))
        {
            yield return state;
        }
    }

    public static TResult GetByFirst<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TSource[], TResult> getMethod)
    {
        using var enumerator = source.GetEnumerator();

        if (enumerator.MoveNext())
        {
            return getMethod(enumerator.Current, enumerator.ReadToEnd().ToArray());
        }
        else
        {
            throw new Exception("Empty source");
        }
    }


    public static Maybe<TSource> SingleMaybe<TSource>(this IEnumerable<TSource> source)
    {
        using var enumerator = source.GetEnumerator();

        if (!enumerator.MoveNext())
        {
            return Maybe<TSource>.Nothing;
        }

        var value = enumerator.Current;

        return Maybe.Maybe.OfCondition(!enumerator.MoveNext(), () => value);
    }

    public static void Foreach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (var item in source)
        {
            action(item);
        }
    }

    public static IReadOnlyList<T> ToReadOnlyListI<T>(this IEnumerable<T> source)
    {
        return source.ToList();
    }
    
    public static IEnumerable<TResult> ZipStrong<TSource, TOther, TResult>(this IEnumerable<TSource> source, IEnumerable<TOther> other, Func<TSource, TOther, TResult> resultSelector)
    {
        using var sourceEnumerator = source.GetEnumerator();
        using var otherEnumerator = other.GetEnumerator();

        while (true)
        {
            var res1 = sourceEnumerator.MoveNext();
            var res2 = otherEnumerator.MoveNext();

            if (res1 != res2)
            {
                throw new InvalidOperationException("The sequences had different lengths");
            }
            else if (res1)
            {
                yield return resultSelector(sourceEnumerator.Current, otherEnumerator.Current);
            }
            else
            {
                yield break;
            }
        }
    }

    public static IEnumerable<T> GetDuplicates<T>(this IEnumerable<T> source, IEqualityComparer<T>? comparer = null)
    {
        var hashSet = new HashSet<T>(comparer ?? EqualityComparer<T>.Default);

        return source.Where(item => !hashSet.Add(item));
    }

    public static TSource Single<TSource>(this IEnumerable<TSource> source, Func<Exception> emptyExceptionHandler, Func<IReadOnlyCollection<TSource>, Exception> manyExceptionHandler)
    {
        return source.Match(
            () => throw emptyExceptionHandler(),
            value => value,
            items => throw manyExceptionHandler(items));
    }

    public static TSource? SingleOrDefault<TSource>(this IEnumerable<TSource> source, Func<Exception> manyExceptionHandler)
    {
        using var enumerator = source.GetEnumerator();

        if (!enumerator.MoveNext())
        {
            return default;
        }

        var current = enumerator.Current;

        if (enumerator.MoveNext())
        {
            throw manyExceptionHandler();
        }

        return current;
    }
}
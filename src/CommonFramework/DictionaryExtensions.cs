using CommonFramework.Maybe;

namespace CommonFramework;

public static class DictionaryExtensions
{
    public static Dictionary<TNewKey, TValue> ChangeKey<TOldKey, TNewKey, TValue>(this IReadOnlyDictionary<TOldKey, TValue> source, Func<TOldKey, TNewKey> selector)
        where TNewKey : notnull
    {
        return source.ToDictionary(pair => selector(pair.Key), pair => pair.Value);
    }

    public static TValue GetValueOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, object syncLocker, Func<TValue> getNewPairValue)
    {
        if (source.TryGetValue(key, out var value))
        {
            return value;
        }

        lock (syncLocker)
        {
            if (source.TryGetValue(key, out value))
            {
                return value;
            }

            value = getNewPairValue();

            source.Add(key, value);

            return value;
        }
    }

    public static Dictionary<TKey, TValue> Concat<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> source, IReadOnlyDictionary<TKey, TValue> other)
        where TKey : notnull
    {
        return ((IEnumerable<KeyValuePair<TKey, TValue>>)source).Concat(other).ToDictionary();
    }

    public static TValue GetValueOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, Func<TValue> getNewPairValue)
    {
        if (source.TryGetValue(key, out var value))
        {
            return value;
        }

        value = getNewPairValue();

        source.Add(key, value);

        return value;
    }

    public static TValue GetValueOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> source, TKey key, Func<TValue> getDefaultValueFunc)
    {
        return source.TryGetValue(key, out var value) ? value : getDefaultValueFunc();
    }

    public static TValue GetValue<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> source, TKey key, Func<Exception> getKeyNotFoundError)
    {
        return source.GetValueOrDefault(key, () => throw getKeyNotFoundError());
    }

    public static Maybe<TValue> GetMaybeValue<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> source, TKey key)
    {
        return source.TryGetValue(key, out var value) ? new Just<TValue>(value) : Maybe<TValue>.Nothing;
    }

    public static Dictionary<TKey, TNewValue> ChangeValue<TKey, TOldValue, TNewValue>(this IReadOnlyDictionary<TKey, TOldValue> source, Func<TOldValue, TNewValue> selector)
        where TKey : notnull
    {
        return source.ToDictionary(pair => pair.Key, pair => selector(pair.Value));
    }
}
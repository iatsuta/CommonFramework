using System.Collections.Concurrent;

namespace CommonFramework;

public static class ConcurrentDictionaryExtensions
{
    public static TValue GetOrAdd<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> concurrentDictionary, TKey key, Func<TValue> valueFactory)
        where TKey : notnull => concurrentDictionary.GetOrAdd(key, _ => valueFactory());
}
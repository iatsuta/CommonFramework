namespace CommonFramework;

public static class DictionaryCacheExtensions
{
    public static IDictionaryCache<TKey, TValue> WithLock<TKey, TValue>(this IDictionaryCache<TKey, TValue> dictionaryCache, object? locker = null)
    {
        return new ConcurrentDictionaryCache<TKey, TValue>(dictionaryCache, locker ?? new object());
    }

    private class ConcurrentDictionaryCache<TKey, TValue>(IDictionaryCache<TKey, TValue> baseDictionaryCache, object locker) : IDictionaryCache<TKey, TValue>
    {
        public TValue this[TKey key]
        {
            get
            {
                lock (locker)
                {
                    return baseDictionaryCache[key];
                }
            }
        }

        public IEnumerable<TValue> Values
        {
            get
            {
                lock (locker)
                {
                    return baseDictionaryCache.Values.ToArray();
                }
            }
        }
    }
}
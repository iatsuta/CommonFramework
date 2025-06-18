namespace CommonFramework;

public static class DictionaryExtensions
{
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
}
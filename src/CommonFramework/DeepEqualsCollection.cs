﻿namespace CommonFramework;

public class DeepEqualsCollection<T>(IReadOnlyList<T> baseSource, IEqualityComparer<T> comparer)
    : IReadOnlyList<T>, IEquatable<DeepEqualsCollection<T>>
{
    public DeepEqualsCollection(IEnumerable<T> baseSource, IEqualityComparer<T>? comparer = null)
        : this(baseSource.ToList(), comparer ?? EqualityComparer<T>.Default)
    {
    }

    public int Count => baseSource.Count;

    public T this[int index] => baseSource[index];

    public IEnumerator<T> GetEnumerator() => baseSource.GetEnumerator();

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => this.GetEnumerator();

    public bool Equals(DeepEqualsCollection<T>? other) => !ReferenceEquals(other, null) && baseSource.SequenceEqual(other, comparer);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;

        return this.Equals(obj as DeepEqualsCollection<T>);
    }

    public override int GetHashCode() => baseSource.Count;

    public static implicit operator DeepEqualsCollection<T>(T[] source) => DeepEqualsCollection.Create(source);
}

public static class DeepEqualsCollection
{
    public static DeepEqualsCollection<T> Create<T>(IEnumerable<T> source) => Create(source, null);

    public static DeepEqualsCollection<T> Create<T>(IEnumerable<T> source, IEqualityComparer<T>? comparer) => new(source, comparer);
}
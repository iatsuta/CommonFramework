using CommonFramework.ExpressionComparers;

using System.Collections.Immutable;
using System.Linq.Expressions;

namespace CommonFramework;

public sealed record LambdaExpressionPath(ImmutableArray<LambdaExpression> Properties)
{
    public LambdaExpressionPath(IEnumerable<LambdaExpression> properties)
        : this([..properties])
    {
    }

    private static readonly IEqualityComparer<LambdaExpression> Comparer = ExpressionComparer.Default;

    private int? hashCode;

    public bool Equals(LambdaExpressionPath? other) =>

        object.ReferenceEquals(this, other)

        || (other is not null && this.Properties.SequenceEqual(other.Properties, Comparer));

    public override int GetHashCode()
    {
        return this.hashCode ??= this.ComputeHashCode();
    }

    private int ComputeHashCode()
    {
        var hash = new HashCode();

        hash.Add(this.Properties.Length);

        foreach (var expr in this.Properties)
            hash.Add(expr, Comparer);

        return hash.ToHashCode();
    }
}
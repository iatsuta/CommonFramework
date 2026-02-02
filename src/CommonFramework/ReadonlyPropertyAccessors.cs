using System.Linq.Expressions;

using CommonFramework.ExpressionComparers;

namespace CommonFramework;

public record ReadonlyPropertyAccessors<TSource, TProperty>(Expression<Func<TSource, TProperty>> Path, Func<TSource, TProperty> Getter)
{
    public ReadonlyPropertyAccessors(Expression<Func<TSource, TProperty>> path)
        : this(path, path.Compile())
    {
    }

    public virtual bool Equals(ReadonlyPropertyAccessors<TSource, TProperty>? other) =>
        object.ReferenceEquals(this, other)
        || (other is not null && ExpressionComparer.Default.Equals(this.Path, other.Path));

    public override int GetHashCode()
    {
        return ExpressionComparer.Default.GetHashCode(this.Path);
    }
}
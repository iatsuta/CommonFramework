using System.Linq.Expressions;

namespace CommonFramework;

public record PropertyAccessors<TSource, TProperty>(
    Expression<Func<TSource, TProperty>> Path,
    Func<TSource, TProperty> Getter,
    Action<TSource, TProperty> Setter)
    : ReadonlyPropertyAccessors<TSource, TProperty>(Path, Getter)
{
    public PropertyAccessors(
        Expression<Func<TSource, TProperty>> path)
        : this(path, path.Compile(), path.ToSetLambdaExpression().Compile())
    {
    }
}
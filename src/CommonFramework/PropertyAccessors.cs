using System.Linq.Expressions;

using CommonFramework.ExpressionComparers;

namespace CommonFramework;

public record PropertyAccessors<TSource, TProperty>(Expression<Func<TSource, TProperty>> Path)
{
	public Func<TSource, TProperty> Getter { get; } = Path.Compile();

	public Action<TSource, TProperty> Setter { get; } = Path.ToSetLambdaExpression().Compile();

	public virtual bool Equals(PropertyAccessors<TSource, TProperty>? other) =>
		object.ReferenceEquals(this, other)
		|| (other is not null && ExpressionComparer.Default.Equals(this.Path, other.Path));

	public override int GetHashCode()
	{
		return ExpressionComparer.Default.GetHashCode(this.Path);
	}
}
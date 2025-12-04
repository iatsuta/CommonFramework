using System.Linq.Expressions;

namespace CommonFramework;

public record PropertyAccessors<TSource, TProperty>(Expression<Func<TSource, TProperty>> Path)
{
	public Func<TSource, TProperty> Getter { get; } = Path.Compile();

	public Action<TSource, TProperty> Setter { get; } = Path.ToSetLambdaExpression().Compile();
}
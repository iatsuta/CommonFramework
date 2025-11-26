using System.Linq.Expressions;

namespace CommonFramework;

public static class ExpressionHelper
{
	public static Expression<Func<T, TResult>> Create<T, TResult>(Expression<Func<T, TResult>> func)
	{
		return func;
	}

	public static Expression<Func<T, T>> GetIdentity<T>()
	{
		return Cache<T>.IdentityExpr;
	}

	public static Expression<Func<T, T, bool>> GetEquality<T>()
	{
		return Cache<T>.EqualityExpr;
	}

	public static Expression<Func<T, bool>> GetEqualityWithExpr<T>(T value)
	{
		var p1 = Expression.Parameter(typeof(T));

		return Expression.Lambda<Func<T, bool>>(Expression.Equal(p1, Expression.Constant(value)), p1);
	}

	private static class Cache<T>
	{
		public static readonly Expression<Func<T, T>> IdentityExpr = x => x;

		public static readonly Expression<Func<T, T, bool>> EqualityExpr = GetEqualityExpr();

		private static Expression<Func<T, T, bool>> GetEqualityExpr()
		{
			var p1 = Expression.Parameter(typeof(T));
			var p2 = Expression.Parameter(typeof(T));

			return Expression.Lambda<Func<T, T, bool>>(Expression.Equal(p1, p2), p1, p2);
		}
	}
}
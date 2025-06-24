using System.Linq.Expressions;

namespace CommonFramework;

public static class ExpressionHelper
{
    public static Expression<Func<T, T>> GetIdentity<T>()
    {
        return x => x;
    }

    public static Expression<Func<T, T, bool>> GetEquality<T>()
    {
        var p1 = Expression.Parameter(typeof(T));
        var p2 = Expression.Parameter(typeof(T));

        return Expression.Lambda<Func<T, T, bool>>(Expression.Equal(p1, p2), p1, p2);
    }

    public static Expression<Func<T, TResult>> Create<T, TResult>(Expression<Func<T, TResult>> func)
    {
        return func;
    }
}
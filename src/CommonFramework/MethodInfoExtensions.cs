using System.Linq.Expressions;
using System.Reflection;

namespace CommonFramework;

public static class MethodInfoExtensions
{
    public static TResult Invoke<TResult>(this MethodInfo methodInfo, object source)
    {
        return methodInfo.Invoke<TResult>(source, []);
    }

    public static TResult Invoke<TResult>(this MethodInfo methodInfo, object source, IEnumerable<object> args)
    {
        return (TResult)methodInfo.Invoke(source, args.ToArray())!;
    }

    public static TResult Invoke<TResult>(this MethodInfo methodInfo, object source, object arg1, params object[] args)
    {
        return methodInfo.Invoke<TResult>(source, new[] { arg1 }.Concat(args));
    }

    public static bool IsGenericMethodImplementation(this MethodInfo methodInfo, MethodInfo genericMethod)
    {
        if (!genericMethod.IsGenericMethodDefinition)
        {
            throw new ArgumentOutOfRangeException(nameof(genericMethod));
        }

        return methodInfo.IsGenericMethod && methodInfo.GetGenericMethodDefinition() == genericMethod;
    }

    public static MethodCallExpression ToCallExpression(this MethodInfo methodInfo, IEnumerable<Expression> children)
    {
        if (methodInfo.IsStatic)
        {
            return Expression.Call(methodInfo, children);
        }
        else
        {
            return children.GetByFirst((first, other) => Expression.Call(first, methodInfo, other));
        }
    }
}
using CommonFramework.Maybe;
using CommonFramework.Visitor;

using System.Linq.Expressions;
using System.Reflection;

namespace CommonFramework;

public static class ExpressionExtensions
{
    public static Expression<Action<TSource, TProperty>> ToSetLambdaExpression<TSource, TProperty>(this Expression<Func<TSource, TProperty>> expr)
    {
        return expr.GetProperty().ToSetLambdaExpression<TSource, TProperty>();
    }

    public static IEnumerable<Expression> GetChildren(this MethodCallExpression expression)
    {
        if (expression == null) throw new ArgumentNullException(nameof(expression));

        if (expression.Object != null)
        {
            yield return expression.Object;
        }

        foreach (var arg in expression.Arguments)
        {
            yield return arg;
        }
    }

    public static Expression<Func<IEnumerable<T>, IEnumerable<T>>> ToCollectionFilter<T>(this Expression<Func<T, bool>> filter)
    {
        var param = Expression.Parameter(typeof(IEnumerable<T>));

        var whereMethod = new Func<IEnumerable<T>, Func<T, bool>, IEnumerable<T>>(Enumerable.Where).Method;

        return Expression.Lambda<Func<IEnumerable<T>, IEnumerable<T>>>(Expression.Call(null, whereMethod, param, filter), param);
    }

    public static PropertyInfo GetProperty<TSource, TResult>(this Expression<Func<TSource, TResult>> expr)
    {
        var request = from member in expr.Body.GetMember()

            from property in (member as PropertyInfo).ToMaybe()

            select property;

        return request.GetValue(() => new ArgumentException("not property expression", nameof(expr)));
    }

    public static Maybe<MemberInfo> GetMember(this Expression expr)
    {
        return (expr as UnaryExpression).ToMaybe().Where(unaryExpr => unaryExpr.NodeType == ExpressionType.Convert)
            .SelectMany(unaryExpr => unaryExpr.Operand.GetMember())

            .Or(() => (expr as MethodCallExpression).ToMaybe().Select(callExpr => (MemberInfo)callExpr.Method))

            .Or(() => (expr as MemberExpression).ToMaybe().Select(memberExpr => memberExpr.Member));
    }

    public static Expression<Func<T, bool>> BuildAnd<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
    {
        return
            from v1 in expr1
            from v2 in expr2
            select v1 && v2;
    }

    public static Expression<Func<T, bool>> BuildAnd<T>(this IEnumerable<Expression<Func<T, bool>>> source)
    {
        return source.Match(() => _ => true,
            single => single,
            many => many.Aggregate(BuildAnd));
    }

    public static Expression<Func<T1, T2, bool>> BuildAnd<T1, T2>(this Expression<Func<T1, T2, bool>> expr1, Expression<Func<T1, T2, bool>> expr2)
    {
        var newExpr2Body = expr2.GetBodyWithOverrideParameters(expr1.Parameters.ToArray<Expression>());

        var newBody = Expression.AndAlso(expr1.Body, newExpr2Body);

        return Expression.Lambda<Func<T1, T2, bool>>(newBody, expr1.Parameters);
    }

    public static Expression<Func<TResult, bool>> BuildOr<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, Expression<Func<TResult, bool>>> selector)
    {
        return source.Select(selector).BuildOr();
    }

    public static Expression<Func<T, bool>> BuildOr<T>(this IEnumerable<Expression<Func<T, bool>>> source)
    {
        return source.Match(() => _ => false,
            single => single,
            many => many.Aggregate(BuildOr));
    }

    public static Expression<Func<T, bool>> BuildOr<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
    {
        return
            from v1 in expr1
            from v2 in expr2
            select v1 || v2;
    }

    public static Expression<Func<T1, T2, bool>> BuildOr<T1, T2>(this Expression<Func<T1, T2, bool>> expr1, Expression<Func<T1, T2, bool>> expr2)
    {
        var newExpr2Body = expr2.GetBodyWithOverrideParameters(expr1.Parameters.ToArray<Expression>());

        var newBody = Expression.OrElse(expr1.Body, newExpr2Body);

        return Expression.Lambda<Func<T1, T2, bool>>(newBody, expr1.Parameters);
    }

    public static Expression<Func<T1, T3>> OverrideInput<T1, T2, T3>(this Expression<Func<T2, T3>> expr2, Expression<Func<T1, T2>> expr1)
    {
        return Expression.Lambda<Func<T1, T3>>(expr2.Body.Override(expr2.Parameters.Single(), expr1.Body), expr1.Parameters);
    }

    public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expr)
    {
        return
            from v in expr
            select !v;
    }

    public static Expression<Func<IEnumerable<TArg>, bool>> ToEnumerableAny<TArg>(this Expression<Func<TArg, bool>> source)
    {
        var param = Expression.Parameter(typeof(IEnumerable<TArg>));

        var anyMethod = new Func<IEnumerable<TArg>, Func<TArg, bool>, bool>(Enumerable.Any).Method;

        var callExpr = Expression.Call(null, anyMethod, param, source);

        return Expression.Lambda<Func<IEnumerable<TArg>, bool>>(callExpr, param);
    }

    public static Expression<TDelegate> Optimize<TDelegate>(this Expression<TDelegate> expression)
    {
        return expression.UpdateBody(OptimizeBooleanLogicVisitor.Value);
    }

    public static Expression<TDelegate> ExpandConst<TDelegate>(this Expression<TDelegate> expression)
    {
        return expression.UpdateBody(ExpandConstVisitor.Value);
    }

    public static Expression<TDelegate> UpdateBody<TDelegate>(this Expression<TDelegate> expression, ExpressionVisitor bodyVisitor)
    {
        return expression.Update(bodyVisitor.Visit(expression.Body), expression.Parameters);
    }

    public static LambdaExpression UpdateBodyBase(this LambdaExpression expression, ExpressionVisitor bodyVisitor)
    {
        return Expression.Lambda(bodyVisitor.Visit(expression.Body), expression.Parameters);
    }

    public static Expression GetBodyWithOverrideParameters(this LambdaExpression lambda, params Expression[] newExpressions)
    {
        var pairs = lambda.Parameters.ZipStrong(newExpressions, (parameter, newExpression) => new { Parameter = parameter, NewExpression = newExpression });
        
        return pairs.Aggregate(lambda.Body, (expr, pair) => expr.Override(pair.Parameter, pair.NewExpression));
    }
    
    public static Expression Override(this Expression baseExpression, Expression oldExpr, Expression newExpr)
    {
        return new OverrideExpressionVisitor(oldExpr, newExpr).Visit(baseExpression)!;
    }

    public static Expression UpdateBase(this Expression source, IEnumerable<ExpressionVisitor> visitors)
    {
        return visitors.Aggregate(source, (expr, visitor) => visitor.Visit(expr));
    }

    public static Expression UpdateBase(this Expression source, params ExpressionVisitor[] visitors)
    {
        return source.UpdateBase((IEnumerable<ExpressionVisitor>)visitors);
    }

    public static Maybe<TValue?> GetDeepMemberConstValue<TValue>(this Expression expression)
    {
        return GetDeepMemberConstValue(expression).Where(v => v is TValue).Select(v => (TValue?)v);
    }

    public static Maybe<object?> GetDeepMemberConstValue(this Expression expression)
    {
        return expression.GetDeepMemberConstExpression().Select(expr => expr.Value);
    }
    
    public static Maybe<ConstantExpression> GetMemberConstExpression(this Expression expression)
    {
        return (expression as ConstantExpression).ToMaybe()

            .Or(() =>
                
                from memberExpr in (expression as MemberExpression).ToMaybe()

                from constExpr in (memberExpr.Expression as ConstantExpression).ToMaybe()

                from fieldInfo in (memberExpr.Member as FieldInfo).ToMaybe()

                select Expression.Constant(fieldInfo.GetValue(constExpr.Value), fieldInfo.FieldType));
    }    
    
    /// <summary> Returns constant value from expression
    /// </summary>
    /// <typeparam name="TValue">cast value to specified Type if possible</typeparam>
    /// <param name="expression">expression to get value from</param>
    /// <returns>constant value of specified Type</returns>
    public static Maybe<TValue?> GetMemberConstValue<TValue>(this Expression expression)
    {
        return expression.GetMemberConstValue().Where(v => v is TValue).Select(v => (TValue?)v);
    }

    public static Maybe<object?> GetMemberConstValue(this Expression expression)
    {
        return expression.GetMemberConstExpression().Select(expr => expr.Value);
    }

    public static Maybe<ConstantExpression> GetDeepMemberConstExpression(this Expression expression)
    {
        var result = expression.GetPureDeepMemberConstExpression();
        if (result == null)
        {
            return Maybe<ConstantExpression>.Nothing;
        }

        return expression is ConstantExpression constExpr ? constExpr.ToMaybe() : Maybe.Maybe.Return(result);
    }

    public static ConstantExpression? GetPureDeepMemberConstExpression(this Expression expression)
    {
        if (expression is ConstantExpression constExpr)
        {
            return constExpr;
        }

        if (expression is not MemberExpression memberExpr)
        {
            return null;
        }

        var memberChains = memberExpr.GetAllElements(z => z.Expression as MemberExpression).TakeWhile(x => x != null).ToList();

        var startExpr = memberChains.Last();

        constExpr = startExpr.Expression as ConstantExpression;
        if (constExpr == null)
        {
            return constExpr;
        }

        var constValue = ValueTuple.Create(startExpr.Member.GetValue(constExpr.Value), startExpr.Member.GetMemberType());
        memberChains.Reverse();
        var finalValue = memberChains
            .Skip(1) // выше обработали самый первый (var startExpr = memberChains.Last();)
            .Select(z => z.Member)
            .Aggregate(
                constValue,
                (prevValue, memberInfo) => ValueTuple.Create(memberInfo.GetValue(prevValue.Item1), memberInfo.GetMemberType()));

        if (finalValue.Item1 == null && finalValue.Item2.IsValueType)
        {
            return null;
        }

        return Expression.Constant(finalValue.Item1, finalValue.Item2);
    }

    private static object? GetValue(this MemberInfo source, object? arg)
    {
        return source switch
        {
            FieldInfo field => field.GetValue(arg),
            PropertyInfo property => property.GetValue(arg),
            _ => throw new ArgumentOutOfRangeException(nameof(source))
        };
    }

    private static Type GetMemberType(this MemberInfo source)
    {
        return source switch
        {
            FieldInfo field => field.FieldType,
            PropertyInfo property => property.PropertyType,
            _ => throw new ArgumentOutOfRangeException(nameof(source))
        };
    }

    internal static Func<object, object, object>? GetBinaryMethod(this ExpressionType type)
    {
        switch (type)
        {
            case ExpressionType.Equal:
                return (v1, v2) => Equals(v1, v2);

            case ExpressionType.NotEqual:
                return (v1, v2) => !Equals(v1, v2);

            case ExpressionType.OrElse:
                return (v1, v2) => ((bool)v1) || ((bool)v2);

            case ExpressionType.AndAlso:
                return (v1, v2) => ((bool)v1) && ((bool)v2);

            default:
                return null;
        }
    }
}
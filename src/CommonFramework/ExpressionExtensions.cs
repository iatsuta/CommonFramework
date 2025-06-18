using System.Linq.Expressions;
using System.Reflection;

namespace CommonFramework;

public static class ExpressionExtensions
{
    public static Maybe<object?> GetMemberConstValue(this Expression expression)
    {
        return expression.GetMemberConstExpression().Select(expr => expr.Value);
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
}

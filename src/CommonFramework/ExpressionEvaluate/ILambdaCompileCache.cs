using System.Linq.Expressions;

namespace CommonFramework.ExpressionEvaluate;

public interface ILambdaCompileCache
{
    TDelegate GetFunc<TDelegate>(Expression<TDelegate> lambdaExpression);
}
using System.Linq.Expressions;

namespace CommonFramework.ExpressionEvaluate;

public class ExpressionEvaluator(ILambdaCompileCache lambdaCompileCache) : IExpressionEvaluator
{
    public TDelegate Compile<TDelegate>(Expression<TDelegate> expression) => lambdaCompileCache.GetFunc(expression);
}
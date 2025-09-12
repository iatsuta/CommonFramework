using System.Linq.Expressions;

namespace CommonFramework.ExpressionEvaluate;

public class DefaultExpressionEvaluator : IExpressionEvaluator
{
    public TDelegate Compile<TDelegate>(Expression<TDelegate> expression)
    {
        return expression.Compile();
    }
}
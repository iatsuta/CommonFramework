using System.Linq.Expressions;

namespace CommonFramework.ExpressionEvaluate;

public class FakeExpressionEvaluator : IExpressionEvaluator
{
    public TDelegate Compile<TDelegate>(Expression<TDelegate> expression)
    {
        throw new NotImplementedException();
    }
}
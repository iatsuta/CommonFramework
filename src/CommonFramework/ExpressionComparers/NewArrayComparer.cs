using System.Linq.Expressions;

namespace CommonFramework.ExpressionComparers;

public class NewArrayComparer(ExpressionComparer rootComparer) : ExpressionComparer<NewArrayExpression>
{
	protected override bool PureEquals(NewArrayExpression x, NewArrayExpression y)
    {
        return x.Expressions.SequenceEqual(y.Expressions, rootComparer);
    }
}

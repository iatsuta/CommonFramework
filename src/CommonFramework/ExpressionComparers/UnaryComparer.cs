using System.Linq.Expressions;

namespace CommonFramework.ExpressionComparers;

public class UnaryComparer(ExpressionComparer rootComparer) : ExpressionComparer<UnaryExpression>
{
	protected override bool PureEquals(UnaryExpression x, UnaryExpression y)
    {
        return x.Method == y.Method
               && rootComparer.Equals(x.Operand, y.Operand);
    }
}

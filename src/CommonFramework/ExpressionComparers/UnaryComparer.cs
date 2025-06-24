using System.Linq.Expressions;

namespace CommonFramework.ExpressionComparers;

internal class UnaryComparer : ExpressionComparer<UnaryExpression>
{
    protected override bool PureEquals(UnaryExpression x, UnaryExpression y)
    {
        return x.Method == y.Method
               && ExpressionComparer.Value.Equals(x.Operand, y.Operand);
    }


    public static readonly UnaryComparer Value = new UnaryComparer();
}

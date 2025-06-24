using System.Linq.Expressions;

namespace CommonFramework.ExpressionComparers;

internal class ConditionalComparer : ExpressionComparer<ConditionalExpression>
{
    protected override bool PureEquals(ConditionalExpression x, ConditionalExpression y)
    {
        return ExpressionComparer.Value.Equals(x.Test, y.Test)
               && ExpressionComparer.Value.Equals(x.IfTrue, y.IfTrue)
               && ExpressionComparer.Value.Equals(x.IfFalse, y.IfFalse);
    }


    public static readonly ConditionalComparer Value = new ConditionalComparer();
}
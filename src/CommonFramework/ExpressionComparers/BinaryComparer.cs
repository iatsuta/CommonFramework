using System.Linq.Expressions;

namespace CommonFramework.ExpressionComparers;

internal class BinaryComparer : ExpressionComparer<BinaryExpression>
{
    private BinaryComparer()
    {
    }

    protected override bool PureEquals(BinaryExpression x, BinaryExpression y)
    {
        return x.Method == y.Method
               && ExpressionComparer.Value.Equals(x.Left, y.Left)
               && ExpressionComparer.Value.Equals(x.Right, y.Right);
    }


    public static readonly BinaryComparer Value = new BinaryComparer();
}

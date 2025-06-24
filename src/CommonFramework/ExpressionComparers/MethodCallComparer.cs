using System.Linq.Expressions;

namespace CommonFramework.ExpressionComparers;

internal class MethodCallComparer : ExpressionComparer<MethodCallExpression>
{
    protected override bool PureEquals(MethodCallExpression x, MethodCallExpression y)
    {
        return x.Method == y.Method
               && ExpressionComparer.Value.Equals(x.Object, y.Object)
               && x.Arguments.SequenceEqual(y.Arguments, ExpressionComparer.Value);
    }

    public override int GetHashCode(MethodCallExpression obj)
    {
        return base.GetHashCode(obj) ^ obj.Arguments.Count;
    }

    public static readonly MethodCallComparer Value = new MethodCallComparer();
}

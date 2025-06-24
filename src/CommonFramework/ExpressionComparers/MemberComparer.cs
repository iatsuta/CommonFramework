using System.Linq.Expressions;

namespace CommonFramework.ExpressionComparers;

internal class MemberComparer : ExpressionComparer<MemberExpression>
{
    protected override bool PureEquals(MemberExpression x, MemberExpression y)
    {
        return x.Member == y.Member && ExpressionComparer.Value.Equals(x.Expression, y.Expression);
    }

    public override int GetHashCode(MemberExpression obj)
    {
        return base.GetHashCode(obj) ^ obj.Member.GetHashCode();
    }

    public static readonly MemberComparer Value = new MemberComparer();
}

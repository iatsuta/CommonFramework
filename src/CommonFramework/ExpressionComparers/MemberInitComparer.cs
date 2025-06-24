using System.Linq.Expressions;

namespace CommonFramework.ExpressionComparers;

internal class MemberInitComparer : ExpressionComparer<MemberInitExpression>
{
    protected override bool PureEquals(MemberInitExpression x, MemberInitExpression y)
    {
        return ExpressionComparer.Value.Equals(x.NewExpression, y.NewExpression) && x.Bindings.SequenceEqual(y.Bindings, MemberBindingComparer.Value);
    }


    public static readonly MemberInitComparer Value = new MemberInitComparer();
}
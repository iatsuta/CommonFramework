using System.Linq.Expressions;
using CommonFramework.Maybe;

namespace CommonFramework.ExpressionComparers;

internal class MemberBindingComparer : IEqualityComparer<MemberBinding>
{
    private MemberBindingComparer()
    {

    }


    public bool Equals(MemberBinding? preX, MemberBinding? preY)
    {
        if (ReferenceEquals(preX, preY)) return true;
        if (preX is null || preY is null) return false;

        var baseEquals = preX.BindingType == preY.BindingType && preX.Member == preY.Member;

        return baseEquals &&

               (from x in (preX as MemberAssignment).ToMaybe()

                 from y in (preY as MemberAssignment).ToMaybe()

                 select ExpressionComparer.Value.Equals(x.Expression, y.Expression))

               .Or(() => from x in (preX as MemberListBinding).ToMaybe()

                         from y in (preY as MemberListBinding).ToMaybe()

                         select x.Initializers.SequenceEqual(y.Initializers, ElementInitComparer.Value))

               .Or(() => from x in (preX as MemberMemberBinding).ToMaybe()

                         from y in (preY as MemberMemberBinding).ToMaybe()

                         select x.Bindings.SequenceEqual(y.Bindings, this))

               .GetValueOrDefault();
    }

    public int GetHashCode(MemberBinding memberBinding)
    {
        return memberBinding.BindingType.GetHashCode() ^ memberBinding.Member.GetHashCode();
    }


    public static readonly MemberBindingComparer Value = new MemberBindingComparer();
}

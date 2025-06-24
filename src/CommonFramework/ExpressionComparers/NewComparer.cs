using System.Linq.Expressions;

namespace CommonFramework.ExpressionComparers;

internal class NewComparer : ExpressionComparer<NewExpression>
{
    protected override bool PureEquals(NewExpression x, NewExpression y)
    {
        return x.Arguments.SequenceEqual(y.Arguments, ExpressionComparer.Value)

               && ((x.Members == null && y.Members == null)

                   || (x.Members != null && y.Members != null && x.Members.SequenceEqual(y.Members)));
    }


    public static readonly NewComparer Value = new NewComparer();
}
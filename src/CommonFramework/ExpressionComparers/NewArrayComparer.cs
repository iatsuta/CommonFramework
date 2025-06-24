using System.Linq.Expressions;

namespace CommonFramework.ExpressionComparers;

internal class NewArrayComparer : ExpressionComparer<NewArrayExpression>
{
    protected override bool PureEquals(NewArrayExpression x, NewArrayExpression y)
    {
        return x.Expressions.SequenceEqual(y.Expressions, ExpressionComparer.Value);
    }


    public static readonly NewArrayComparer Value = new NewArrayComparer();
}

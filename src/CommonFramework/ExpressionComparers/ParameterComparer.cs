using System.Linq.Expressions;

namespace CommonFramework.ExpressionComparers;

internal class ParameterComparer : ExpressionComparer<ParameterExpression>
{
    protected override bool PureEquals(ParameterExpression x, ParameterExpression y)
    {
        return x.Name == y.Name;
    }


    public static readonly ParameterComparer Value = new ParameterComparer();
}

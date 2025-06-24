using System.Linq.Expressions;

namespace CommonFramework.ExpressionComparers;

public class LambdaComparer : ExpressionComparer<LambdaExpression>
{
    protected override bool PureEquals(LambdaExpression x, LambdaExpression y)
    {
        return (x.Parameters.SequenceEqual(y.Parameters, ParameterComparer.Value) && ExpressionComparer.Value.Equals(x.Body, y.Body));
    }

    public override int GetHashCode(LambdaExpression obj)
    {
        return base.GetHashCode(obj) ^ obj.Parameters.Count;
    }

    public static readonly LambdaComparer Value = new LambdaComparer();
}
using System.Linq.Expressions;

namespace CommonFramework.ExpressionComparers;

public abstract class ExpressionComparer<TExpression> : IEqualityComparer<TExpression>
        where TExpression : Expression
{
    public bool Equals(TExpression? x, TExpression? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null || y is null) return false;

        return x.NodeType == y.NodeType && x.Type == y.Type && x.GetType() == y.GetType() && this.PureEquals(x, y);
    }

    protected abstract bool PureEquals(TExpression x, TExpression y);

    public virtual int GetHashCode(TExpression obj)
    {
        return obj.Type.GetHashCode() ^ obj.NodeType.GetHashCode();
    }
}

public class ExpressionComparer : ExpressionComparer<Expression>
{
    private ExpressionComparer()
    {
    }

    protected override bool PureEquals(Expression x, Expression y)
    {
        return x switch
        {
            LambdaExpression expression when y is LambdaExpression lambdaExpression => LambdaComparer.Value.Equals(expression, lambdaExpression),
            MethodCallExpression expression when y is MethodCallExpression callExpression => MethodCallComparer.Value.Equals(expression, callExpression),
            MemberExpression expression when y is MemberExpression memberExpression => MemberComparer.Value.Equals(expression, memberExpression),
            BinaryExpression expression when y is BinaryExpression binaryExpression => BinaryComparer.Value.Equals(expression, binaryExpression),
            ParameterExpression expression when y is ParameterExpression parameterExpression => ParameterComparer.Value.Equals(expression, parameterExpression),
            ConstantExpression expression when y is ConstantExpression constantExpression => ConstantComparer.Value.Equals(expression, constantExpression),
            UnaryExpression expression when y is UnaryExpression unaryExpression => UnaryComparer.Value.Equals(expression, unaryExpression),
            NewArrayExpression expression when y is NewArrayExpression arrayExpression => NewArrayComparer.Value.Equals(expression, arrayExpression),
            NewExpression expression when y is NewExpression newExpression => NewComparer.Value.Equals(expression, newExpression),
            MemberInitExpression expression when y is MemberInitExpression initExpression => MemberInitComparer.Value.Equals(expression, initExpression),
            ConditionalExpression expression when y is ConditionalExpression conditionalExpression => ConditionalComparer.Value.Equals(expression, conditionalExpression),
            ListInitExpression expression when y is ListInitExpression initExpression => ListInitComparer.Value.Equals(expression, initExpression),
            _ => throw new NotImplementedException($"Comparison between expression types '{x?.GetType().Name ?? "null"}' and '{y?.GetType().Name ?? "null"}' is not implemented.")
        };
    }


    public static readonly ExpressionComparer Value = new ExpressionComparer();
}
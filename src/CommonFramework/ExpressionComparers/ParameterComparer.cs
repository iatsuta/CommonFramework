using System.Linq.Expressions;

namespace CommonFramework.ExpressionComparers;

public class ParameterComparer : ExpressionComparer<ParameterExpression>
{
	protected override bool PureEquals(ParameterExpression x, ParameterExpression y)
	{
		return x.Name == y.Name;
	}
}
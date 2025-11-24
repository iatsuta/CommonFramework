using System.Collections.Concurrent;
using System.Linq.Expressions;

using CommonFramework.ExpressionComparers;

namespace CommonFramework.ExpressionEvaluate;

public class LambdaCompileCache(LambdaCompileMode mode) : ILambdaCompileCache
{
	private readonly ConcurrentDictionary<Type, ConcurrentDictionary<LambdaExpression, Delegate>> rootCache = new();

	public TDelegate GetFunc<TDelegate>(Expression<TDelegate> lambdaExpression)
	{
		IReadOnlyCollection<ValueTuple<ParameterExpression, ConstantExpression>> args = null!;

		var newLambdaExpression =

			lambdaExpression.Pipe(mode.HasFlag(LambdaCompileMode.OptimizeBooleanLogic), lambda => lambda.Optimize())
				.Pipe(lambda => ConstantToParameters(lambda, out args));

		var getDelegateFunc = this.GetGetDelegate<TDelegate>(newLambdaExpression, args!.Select(v => v.Item1));

		return (TDelegate)getDelegateFunc.DynamicInvoke(args.Select(v => v.Item2.Value).ToArray())!;
	}

	private Delegate GetGetDelegate<TDelegate>(LambdaExpression expr, IEnumerable<ParameterExpression> parameters)
	{
		return
			this.rootCache
				.GetOrAdd(typeof(TDelegate), _ => new ConcurrentDictionary<LambdaExpression, Delegate>(LambdaComparer.Value))
				.GetOrAdd(expr, _ =>
					expr.Pipe(mode.HasFlag(LambdaCompileMode.IgnoreStringCase),
							lambda => lambda.UpdateBodyBase(new OverrideStringEqualityExpressionVisitor(StringComparison.CurrentCultureIgnoreCase)))
						.Pipe(mode.HasFlag(LambdaCompileMode.InjectMaybe), InjectMaybeVisitor.Value.VisitAndGetValueOrDefaultBase)
						.Pipe(body => Expression.Lambda(body, parameters))
						.Compile());
	}

	private static LambdaExpression ConstantToParameters(LambdaExpression lambdaExpression, out IReadOnlyCollection<ValueTuple<ParameterExpression, ConstantExpression>> args)
	{
		var listArgs = new List<ValueTuple<ParameterExpression, ConstantExpression>>();

		var newExpression = lambdaExpression.UpdateBase(new ConstantToParameterExpressionVisitor(listArgs));

		args = listArgs;

		return (LambdaExpression)newExpression;
	}

	private class ConstantToParameterExpressionVisitor(List<ValueTuple<ParameterExpression, ConstantExpression>> args) : ExpressionVisitor
	{
		protected override Expression VisitConstant(ConstantExpression node)
		{
			var newParameter = Expression.Parameter(node.Type, "OverrideConst_" + args.Count);

			args.Add(ValueTuple.Create(newParameter, node));

			return newParameter;
		}
	}
}
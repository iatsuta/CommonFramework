namespace CommonFramework.ExpressionEvaluate;

public interface IExpressionEvaluatorStorage
{
    IExpressionEvaluator GetForType(Type type);
}
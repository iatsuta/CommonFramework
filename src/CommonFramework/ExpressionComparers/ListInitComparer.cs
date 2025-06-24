using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace CommonFramework.ExpressionComparers;

internal class ListInitComparer : ExpressionComparer<ListInitExpression>
{
    protected override bool PureEquals(ListInitExpression x, ListInitExpression y)
    {
        return ExpressionComparer.Value.Equals(x.NewExpression, y.NewExpression)
               && CompareElementInitList(x.Initializers, y.Initializers);
    }

    private static bool CompareElementInitList(ReadOnlyCollection<ElementInit> x, ReadOnlyCollection<ElementInit> y)
    {
        return x == y || x.SequenceEqual(y, ElementInitComparer.Value);
    }


    public static readonly ListInitComparer Value = new ListInitComparer();
}

using System.Linq.Expressions;

namespace CommonFramework.ExpressionComparers;

internal class ElementInitComparer : IEqualityComparer<ElementInit>
{
    private ElementInitComparer()
    {

    }


    public bool Equals(ElementInit? x, ElementInit? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null || y is null) return false;

        return x.AddMethod == y.AddMethod
               && x.Arguments.SequenceEqual(y.Arguments, ExpressionComparer.Value);
    }

    public int GetHashCode(ElementInit obj)
    {
        return obj.AddMethod.GetHashCode();
    }


    public static readonly ElementInitComparer Value = new ElementInitComparer();
}

﻿using System.Linq.Expressions;

namespace CommonFramework.ExpressionComparers;

internal class ConstantComparer : ExpressionComparer<ConstantExpression>
{
    protected override bool PureEquals(ConstantExpression x, ConstantExpression y)
    {
        return Equals(x.Value, y.Value);
    }


    public static readonly ConstantComparer Value = new ConstantComparer();
}

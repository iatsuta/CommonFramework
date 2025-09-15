using CommonFramework.ExpressionEvaluate;

using System.Linq.Expressions;

namespace CommonFramework.Tests;

public class MaybeTests
{
    [Theory]
    [MemberData(nameof(GetTestCases))]
    public void InjectMaybe_Works_AsExpected(A input, string? expected)
    {
        // arrange
        Expression<Func<A, string?>> expr = a => a.Parent!.Name;
        var injectMaybeExpr = InjectMaybeVisitor.Value.VisitAndGetValueOrDefault(expr);
        var func = injectMaybeExpr.Compile();

        // act
        var result = func(input);

        // assert
        result.Should().Be(expected);
    }

    public static IEnumerable<object?[]> GetTestCases()
    {
        yield return [null, null];
        yield return [new A { Name = "A1" }, null];
        yield return [new A { Name = "A1", Parent = new A { Name = "A2" } }, "A2"];
    }

    public class A
    {
        public A? Parent { get; set; }
        public required string Name { get; set; }
    }
}
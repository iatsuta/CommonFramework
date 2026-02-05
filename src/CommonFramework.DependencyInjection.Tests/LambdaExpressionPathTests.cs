using System.Linq.Expressions;

namespace CommonFramework.DependencyInjection.Tests;

public class LambdaExpressionPathTests
{
    [Fact]
    public void LambdaExpressionPath_ShouldBeEqual_ForDifferentInstancesWithSameExpressions()
    {
        // Arrange: create two arrays of identical expressions
        var array1 = new Expression<Func<int, int>>[] { x => x + 1, y => y - 2 };
        var array2 = new Expression<Func<int, int>>[] { x => x + 1, y => y - 2 };

        var path1 = new LambdaExpressionPath(array1);
        var path2 = new LambdaExpressionPath(array2);

        // Act & Assert
        path1.Should().BeEquivalentTo(path2, "two different instances with identical expressions should be structurally equal");

        // Check Equals
        path1.Equals(path2).Should().BeTrue();

        // Check reference equality (should be different objects)
        ReferenceEquals(path1, path2).Should().BeFalse();

        // Check hash code
        path1.GetHashCode().Should().Be(path2.GetHashCode(), "structurally equal paths must have identical hash codes");
    }
}
using System.Linq.Expressions;
using System.Reflection;

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

    [Fact]
    public void Create_ShouldResolveProperties_ForPathWithCollection()
    {
        // arrange
        var path = "Items.SubItem.Value";

        // act
        var parsedPath = LambdaExpressionPath.Create(typeof(ObjA), path.Split('.'));

        // assert
        parsedPath.Properties.Select(expr => expr.GetProperty())
            .Should()
            .BeEquivalentTo(
            [
                typeof(ObjA).GetRequiredProperty(nameof(ObjA.Items), BindingFlags.Public | BindingFlags.Instance),
                typeof(ObjB).GetRequiredProperty(nameof(ObjB.SubItem), BindingFlags.Public | BindingFlags.Instance),
                typeof(ObjC).GetRequiredProperty(nameof(ObjC.Value), BindingFlags.Public | BindingFlags.Instance)
            ]);
    }

    public record ObjA(IEnumerable<ObjB> Items);

    public record ObjB(ObjC SubItem);

    public record ObjC(int Value);
}
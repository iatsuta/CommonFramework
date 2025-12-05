using CommonFramework.ExpressionComparers;
using CommonFramework.VisualIdentitySource.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

namespace CommonFramework.VisualIdentitySource.Tests;

public class VisualIdentitySourceTests
{
	[Fact]
	public void GetDefaultVisualIdentityInfo_ResultCorrected()
	{
		//Arrange
		var nameLambda = ExpressionHelper.Create((TestObject1 v) => v.Name);

		var sp = new ServiceCollection()
			.AddVisualIdentitySource()
			.BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true, ValidateOnBuild = true });

		var service = sp.GetRequiredService<IVisualIdentityInfoSource>();

		var expectedResult = nameLambda.GetProperty().ToLambdaExpression();

		//Act
		var result = service.GetVisualIdentityInfo<TestObject1>();

		//Assert
		result.Name.Path.Should().Be(expectedResult, ExpressionComparer.Default);
	}


	[Fact]
	public void GetCustomVisualIdentityInfo_ResultCorrected()
	{
		//Arrange
		var nameLambda = ExpressionHelper.Create((TestObject2 v) => v.MyName);

		var sp = new ServiceCollection()
			.AddVisualIdentitySource(b => b.SetName(nameLambda))
			.BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true, ValidateOnBuild = true });

		var service = sp.GetRequiredService<IVisualIdentityInfoSource>();

		var expectedResult = new VisualIdentityInfo<TestObject2>(nameLambda);

		//Act
		var result = service.GetVisualIdentityInfo<TestObject2>();

		//Assert
		result.Should().Be(expectedResult);
	}


	public class TestObject1
	{
		public string Name { get; set; }
	}

	public class TestObject2
	{
		public string MyName { get; set; }
	}
}
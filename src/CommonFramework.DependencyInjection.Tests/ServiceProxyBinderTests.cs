using Microsoft.Extensions.DependencyInjection;

namespace CommonFramework.DependencyInjection.Tests;

public sealed class ServiceProxyBinderTests
{
    private interface IService<T>;

    private class Service<T, THiddenGeneric> : IService<T>;


    [Fact]
    public void Should_Create_ServiceProxy_Using_CustomBinder()
    {
        // arrange
        var sp = new ServiceCollection()
            .BindServiceProxy(typeof(IService<>), typeof(ServiceProxyBinder<>))
            .BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true, ValidateOnBuild = true });

        var serviceProxyFactory = sp.GetServiceProxyFactory();

        // act
        var service = serviceProxyFactory.Create<IService<int>>();

        // assert
        service.Should().BeOfType<Service<int, string>>();
    }

    [Fact]
    public void Should_Replace_Previous_ServiceProxy_Binding_When_Replace_Is_True()
    {
        // arrange
        var sp = new ServiceCollection()
            .BindServiceProxy(typeof(IService<>), typeof(ServiceProxyBinder<>))
            .BindServiceProxy(typeof(IService<>), typeof(ServiceProxyBinder<>), replace: true)
            .BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true, ValidateOnBuild = true });

        var serviceProxyFactory = sp.GetServiceProxyFactory();

        // act
        var service = serviceProxyFactory.Create<IService<int>>();

        // assert
        service.Should().BeOfType<Service<int, string>>();
    }

    [Fact]
    public void Should_Use_Specific_ServiceProxy_Binding_When_Replacing_Open_Generic_Binding()
    {
        // arrange
        var sp = new ServiceCollection()
            .BindServiceProxy(typeof(IService<>), typeof(ServiceProxyBinder<>))
            .BindServiceProxy(typeof(IService<int>), typeof(ServiceProxyBinder<int>), replace: true)
            .BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true, ValidateOnBuild = true });

        var serviceProxyFactory = sp.GetServiceProxyFactory();

        // act
        var service = serviceProxyFactory.Create<IService<int>>();

        // assert
        service.Should().BeOfType<Service<int, string>>();
    }

    [Fact]
    public void Should_Throw_When_Multiple_ServiceProxy_Bindings_Are_Registered_Without_Replace()
    {
        // arrange
        var sp = new ServiceCollection()
            .BindServiceProxy(typeof(IService<>), typeof(ServiceProxyBinder<>))
            .BindServiceProxy(typeof(IService<>), typeof(ServiceProxyBinder<>))
            .BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true, ValidateOnBuild = true });

        var serviceProxyFactory = sp.GetServiceProxyFactory();

        // act
        var act = () => serviceProxyFactory.Create<IService<int>>();

        // assert
        act.Should().Throw().WithMessage("Each subsequent candidate must replace the previous candidate");
    }

    [Fact]
    public void Should_Throw_When_Specific_ServiceProxy_Binding_Does_Not_Replace_Previous_Binding()
    {
        // arrange
        var sp = new ServiceCollection()
            .BindServiceProxy(typeof(IService<>), typeof(ServiceProxyBinder<>))
            .BindServiceProxy(typeof(IService<int>), typeof(ServiceProxyBinder<int>))
            .BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true, ValidateOnBuild = true });

        var serviceProxyFactory = sp.GetServiceProxyFactory();

        // act
        var act = () => serviceProxyFactory.Create<IService<int>>();

        // assert
        act.Should().Throw().WithMessage("Each subsequent candidate must replace the previous candidate");
    }

    private class ServiceProxyBinder<T> : IServiceProxyBinder
    {
        public Type GetTargetServiceType()
        {
            return typeof(Service<T, string>);
        }
    }
}
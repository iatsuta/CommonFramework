using Microsoft.Extensions.DependencyInjection;

namespace CommonFramework.DependencyInjection.Tests;

public class DuplicateServiceUsageValidatorTests
{
    [Fact]
    public void Validate_ShouldThrow_WhenMultipleServicesUsedAsSingleInstanceDependency()
    {
        var services = new ServiceCollection();

        services.AddSingleton<IService, ServiceImpl>();
        services.AddSingleton<IService, ServiceImpl>();

        services.AddSingleton<Consumer>();

        services.AddValidator(new DuplicateServiceUsageValidator());

        Action act = () => services.Validate();

        act.Should().Throw<InvalidOperationException>()
            .Where(ex => ex.Message.Contains("has been registered many times"));
    }

    [Fact]
    public void Validate_ShouldNotThrow_WhenServiceUsedAsCollectionDependency()
    {
        var services = new ServiceCollection();

        services.AddSingleton<IService, ServiceImpl>();
        services.AddSingleton<IService, ServiceImpl>();

        services.AddSingleton<ConsumerWithCollection>();

        services.AddValidator(new DuplicateServiceUsageValidator());

        Action act = () => services.Validate();

        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_ShouldNotThrow_WhenDuplicateServiceIsExcludedByValidator()
    {
        var services = new ServiceCollection();

        services.AddSingleton<IService, ServiceImpl>();
        services.AddSingleton<IService, ServiceImpl>();

        services.AddSingleton<Consumer>();

        services.AddValidator(new DuplicateServiceUsageValidator([typeof(IService)]));
        Action act = () => services.Validate();

        act.Should().NotThrow();
    }

    private interface IService;

    private class ServiceImpl : IService;

    private class Consumer(IService service);

    private class ConsumerWithCollection(IEnumerable<IService> services);
}
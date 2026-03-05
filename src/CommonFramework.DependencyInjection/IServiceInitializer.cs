using Microsoft.Extensions.DependencyInjection;

namespace CommonFramework.DependencyInjection;

public interface IServiceInitializer<in TService>
{
    void Initialize(TService service);
}

public interface IServiceInitializer : IServiceInitializer<IServiceCollection>;
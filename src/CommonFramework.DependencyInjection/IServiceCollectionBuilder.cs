using Microsoft.Extensions.DependencyInjection;

namespace CommonFramework.DependencyInjection;

public interface IServiceCollectionBuilder
{
    void Initialize(IServiceCollection services);
}
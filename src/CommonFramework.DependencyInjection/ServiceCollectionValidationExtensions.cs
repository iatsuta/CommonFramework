﻿using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

namespace CommonFramework.DependencyInjection;

public static class ServiceCollectionValidationExtensions
{
    public static IServiceCollection ValidateDuplicateDeclaration(this IServiceCollection serviceCollection, params Type[] exceptServices)
    {
        var wrongMultiUsage = serviceCollection.GetWrongMultiUsage();

        var filteredWrongMultiUsage = wrongMultiUsage.Where(pair => !exceptServices.Contains(pair.ServiceType)).ToList();

        if (filteredWrongMultiUsage.Any())
        {
            var message = filteredWrongMultiUsage.Join(
                Environment.NewLine,
                pair =>
                {
                    var keyedParts = pair.IsKeyedService ? $" (ServiceKey: {pair.ServiceKey})" : null;

                    return
                        $"The service {pair.ServiceType}{keyedParts} has been registered many times. There are services that use it in the constructor in a single instance: "
                        + pair.UsedFor.Join(", ", usedService => usedService.ImplementationType);
                });

            throw new InvalidOperationException(message);
        }

        return serviceCollection;
    }

    private static List<(Type ServiceType, bool IsKeyedService, object ServiceKey, List<ServiceDescriptor> UsedFor)> GetWrongMultiUsage(this IServiceCollection serviceCollection)
    {
        var usedParametersRequest =

                from service in serviceCollection

                let actualImplementationType = service.IsKeyedService ? service.KeyedImplementationType : service.ImplementationType

                let actualImplementationFactory = service.IsKeyedService ? (object)service.KeyedImplementationFactory : service.ImplementationFactory

                let serviceKey = service.IsKeyedService ? service.ServiceKey : null

                where actualImplementationType != null && actualImplementationFactory == null

                let ctors = actualImplementationType!.GetConstructors()

                let actualCtor = ctors.Length == 1
                                         ? ctors[0]
                                         : ctors
                                           .Where(ctor => ctor.GetCustomAttributes<ActivatorUtilitiesConstructorAttribute>().Any())
                                           .Match(() => null, ctor => ctor, _ => null)

                where actualCtor != null

                from parameterType in actualCtor.GetParameters().Select(p => p.ParameterType).Distinct()

                group service by (parameterType, service.IsKeyedService, serviceKey);

        var usedParametersDict = usedParametersRequest.ToDictionary(g => g.Key, g => g.ToList());



        var wrongMultiUsageRequest =

                from service in serviceCollection

                let serviceKey = service.IsKeyedService ? service.ServiceKey : null

                group service by (service.ServiceType, service.IsKeyedService, serviceKey) into serviceTypeGroup

                where serviceTypeGroup.Count() > 1

                let servicesWithSimpleUsage = usedParametersDict.GetValueOrDefault(serviceTypeGroup.Key)

                where servicesWithSimpleUsage != null

                select (serviceTypeGroup.Key.ServiceType, serviceTypeGroup.Key.IsKeyedService, serviceTypeGroup.Key.serviceKey, servicesWithSimpleUsage);

        return wrongMultiUsageRequest.ToList();
    }
}

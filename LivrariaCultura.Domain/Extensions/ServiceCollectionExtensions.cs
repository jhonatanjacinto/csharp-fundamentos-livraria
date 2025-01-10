using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace LivrariaCultura.Domain.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddScopedFromNamespace(this IServiceCollection services, string assemblyName, string namespacePartialName)
    {
        Assembly.Load(assemblyName)
            .GetTypes()
            .Where(type => type.IsClass == true && !type.IsAbstract && type.Namespace != null && type.Namespace.Contains(namespacePartialName))
            .ToList()
            .ForEach(type => services.AddScoped(type));

        return services;
    }
}
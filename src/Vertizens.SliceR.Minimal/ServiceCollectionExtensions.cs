using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Vertizens.ServiceProxy;

namespace Vertizens.SliceR.Minimal;

/// <summary>
/// Extensions for ServiceCollection
/// </summary>
public static partial class ServiceCollectionExtensions
{
    /// <summary>
    /// Register <see cref="IEndpointBuilder"/> implementation types that define endpoints from calling assembly
    /// </summary>
    public static IServiceCollection AddSliceREndpointBuilders(this IServiceCollection services)
    {
        return services.AddSliceREndpointBuilders(Assembly.GetCallingAssembly());
    }

    /// <summary>
    /// Register <see cref="IEndpointBuilder"/> implementation types that define endpoints from specified <paramref name="assembly"/>.
    /// </summary>
    /// <param name="services">Service Collection</param>
    /// <param name="assembly">Assembly used to register types from</param>
    public static IServiceCollection AddSliceREndpointBuilders(this IServiceCollection services, Assembly assembly)
    {
        services.AddInterfaceTypes(typeof(IEndpointBuilder), assembly);

        return services;
    }
}

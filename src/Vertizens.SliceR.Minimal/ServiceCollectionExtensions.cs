using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Vertizens.ServiceProxy;

namespace Vertizens.SliceR.Minimal;
public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddSliceREndpointBuilders(this IServiceCollection services)
    {
        return services.AddSliceREndpointBuilders(Assembly.GetCallingAssembly());
    }

    public static IServiceCollection AddSliceREndpointBuilders(this IServiceCollection services, Assembly assembly)
    {
        services.AddInterfaceTypes(typeof(IEndpointBuilder), assembly);

        return services;
    }
}

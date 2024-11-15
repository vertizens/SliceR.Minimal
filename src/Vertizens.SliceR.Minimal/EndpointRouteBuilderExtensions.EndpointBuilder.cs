using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Vertizens.SliceR.Minimal;
public static partial class EndpointRouteBuilderExtensions
{
    /// <summary>
    /// Use any registered <see cref="IEndpointBuilder"/> services to define minimal endpoints
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IEndpointRouteBuilder AddEndpointBuilders(this IEndpointRouteBuilder builder)
    {
        var endpointBuilders = builder.ServiceProvider.GetServices<IEndpointBuilder>();

        foreach (var endpointBuilder in endpointBuilders)
        {
            endpointBuilder.Build(builder);
        }

        return builder;
    }

}

using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Vertizens.SliceR.Minimal;
public static partial class EndpointRouteBuilderExtensions
{
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

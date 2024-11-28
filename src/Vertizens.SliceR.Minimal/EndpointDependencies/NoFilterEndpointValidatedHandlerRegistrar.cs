using Microsoft.Extensions.DependencyInjection.Extensions;
using Vertizens.SliceR.Operations;
using Vertizens.SliceR.Validated;

namespace Vertizens.SliceR.Minimal;
internal class NoFilterEndpointValidatedHandlerRegistrar : IEndpointValidatedHandlerRegistrar
{
    public bool Handle(EndpointHandler endpointHandler, ValidatedHandlerRegistrarContext context)
    {
        var handled = false;
        var arguments = endpointHandler.HandlerType.GetGenericArguments();
        if (arguments.Length == 2)
        {
            var request = arguments[0];
            var result = arguments[1];
            if (request == typeof(NoFilter) && result.IsGenericType && result.GetGenericTypeDefinition() == typeof(IQueryable<>))
            {
                var resolveType = result.GetGenericArguments()[0];
                var resolved = HandlerResolvedContext.Create(resolveType, endpointHandler.Endpoint, context.EntityDefinitionResolver, context.EntityMetadataTypeResolver);

                if (resolved.EntityDefinition != null)
                {
                    if (resolved.DomainType != null)
                    {
                        context.Services.TryAddTransient(
                            endpointHandler.HandlerType,
                            typeof(NoFilterQueryableValidatedHandler<>).MakeGenericType(resolved.DomainType));

                        context.EntityDomainHandlerRegistrar.Register(new EntityDomainHandlerContext
                        {
                            Services = context.Services,
                            EntityDefinition = resolved.EntityDefinition!,
                            DomainType = resolved.DomainType!,
                            RequestType = request,
                            ResultType = result
                        });
                    }
                    else
                    {
                        context.Services.TryAddTransient(
                            endpointHandler.HandlerType,
                            typeof(NoFilterQueryableValidatedHandler<>).MakeGenericType(resolved.EntityDefinition.EntityType));
                    }
                    handled = true;
                }
            }
        }

        return handled;
    }
}

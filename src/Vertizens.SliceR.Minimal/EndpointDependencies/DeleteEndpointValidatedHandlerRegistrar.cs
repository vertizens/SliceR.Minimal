using Microsoft.Extensions.DependencyInjection.Extensions;
using Vertizens.SliceR.Operations;
using Vertizens.SliceR.Validated;

namespace Vertizens.SliceR.Minimal;
internal class DeleteEndpointValidatedHandlerRegistrar : IEndpointValidatedHandlerRegistrar
{
    public bool Handle(EndpointHandler endpointHandler, ValidatedHandlerRegistrarContext context)
    {
        var handled = false;
        var arguments = endpointHandler.HandlerType.GetGenericArguments();
        if (arguments.Length == 1)
        {
            if (arguments[0].IsGenericType && arguments[0].GetGenericTypeDefinition() == typeof(Delete<,>))
            {
                var resolveType = arguments[0].GetGenericArguments()[1];
                var resolved = HandlerResolvedContext.Create(resolveType, endpointHandler.Endpoint, context.EntityDefinitionResolver, context.EntityMetadataTypeResolver);

                if (resolved.EntityDefinition != null && resolved.EntityDefinition?.KeyType != null)
                {
                    if (resolved.DomainType != null)
                    {
                        context.Services.TryAddTransient(
                            endpointHandler.HandlerType,
                            typeof(DeleteValidatedHandler<,,>).MakeGenericType(resolved.EntityDefinition.KeyType, resolved.DomainType, resolved.EntityDefinition.EntityType));
                    }
                    else
                    {
                        context.Services.TryAddTransient(
                            endpointHandler.HandlerType,
                            typeof(DeleteValidatedHandler<,>).MakeGenericType(resolved.EntityDefinition.KeyType, resolved.EntityDefinition.EntityType));
                    }
                    handled = true;
                }
            }
        }

        return handled;
    }
}

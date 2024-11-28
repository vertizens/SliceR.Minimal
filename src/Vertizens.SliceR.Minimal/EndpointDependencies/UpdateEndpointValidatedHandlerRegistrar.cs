using Microsoft.Extensions.DependencyInjection.Extensions;
using Vertizens.SliceR.Operations;
using Vertizens.SliceR.Validated;

namespace Vertizens.SliceR.Minimal;
internal class UpdateEndpointValidatedHandlerRegistrar : IEndpointValidatedHandlerRegistrar
{
    public bool Handle(EndpointHandler endpointHandler, ValidatedHandlerRegistrarContext context)
    {
        var handled = false;
        var arguments = endpointHandler.HandlerType.GetGenericArguments();
        if (arguments.Length == 2)
        {
            var request = arguments[0];
            var result = arguments[1];
            if (request.IsGenericType && request.GetGenericTypeDefinition() == typeof(Update<,>))
            {
                var updatedArguments = arguments[0].GetGenericArguments();
                var keyType = updatedArguments[0];
                var updateRequestType = updatedArguments[1];
                var resolveType = arguments[1];
                var resolved = HandlerResolvedContext.Create(resolveType, endpointHandler.Endpoint, context.EntityDefinitionResolver, context.EntityMetadataTypeResolver);

                if (resolved.EntityDefinition != null && resolved.EntityDefinition.KeyType == keyType)
                {
                    if (resolved.DomainType != null)
                    {
                        context.Services.TryAddTransient(
                            endpointHandler.HandlerType,
                            typeof(UpdateValidatedHandler<,,,>).MakeGenericType(keyType, updateRequestType, resolved.DomainType, resolved.EntityDefinition.EntityType));
                    }
                    else
                    {
                        context.Services.TryAddTransient(
                            endpointHandler.HandlerType,
                            typeof(UpdateValidatedHandler<,,>).MakeGenericType(keyType, updateRequestType, resolved.EntityDefinition.EntityType));
                    }

                    context.EntityDomainHandlerRegistrar.Register(new EntityDomainHandlerContext
                    {
                        Services = context.Services,
                        EntityDefinition = resolved.EntityDefinition!,
                        DomainType = resolved.DomainType!,
                        RequestType = request,
                        ResultType = result
                    });

                    handled = true;
                }
            }
        }

        return handled;
    }
}

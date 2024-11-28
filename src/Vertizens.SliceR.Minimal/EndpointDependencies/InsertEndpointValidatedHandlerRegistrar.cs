using Microsoft.Extensions.DependencyInjection.Extensions;
using Vertizens.SliceR.Operations;
using Vertizens.SliceR.Validated;

namespace Vertizens.SliceR.Minimal;
internal class InsertEndpointValidatedHandlerRegistrar : IEndpointValidatedHandlerRegistrar
{
    public bool Handle(EndpointHandler endpointHandler, ValidatedHandlerRegistrarContext context)
    {
        var handled = false;
        var arguments = endpointHandler.HandlerType.GetGenericArguments();
        if (arguments.Length == 2)
        {
            if (arguments[0].IsGenericType && arguments[0].GetGenericTypeDefinition() == typeof(Insert<>))
            {
                var insertRequestType = arguments[0].GetGenericArguments()[0];
                var resolveType = arguments[1];
                var resolved = HandlerResolvedContext.Create(resolveType, endpointHandler.Endpoint, context.EntityDefinitionResolver, context.EntityMetadataTypeResolver);

                if (resolved.EntityDefinition != null)
                {
                    if (resolved.DomainType != null && resolved.EntityDefinition.KeyType != null)
                    {
                        context.Services.TryAddTransient(
                            endpointHandler.HandlerType,
                            typeof(InsertValidatedHandler<,,,>).MakeGenericType(insertRequestType, resolved.DomainType, resolved.EntityDefinition.KeyType, resolved.EntityDefinition.EntityType));
                    }
                    else
                    {
                        context.Services.TryAddTransient(
                            endpointHandler.HandlerType,
                            typeof(InsertValidatedHandler<,>).MakeGenericType(insertRequestType, resolved.EntityDefinition.EntityType));
                    }
                    handled = true;
                }
            }
        }

        return handled;
    }
}

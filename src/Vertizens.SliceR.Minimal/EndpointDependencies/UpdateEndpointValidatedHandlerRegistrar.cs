using Microsoft.Extensions.DependencyInjection.Extensions;
using Vertizens.SliceR.Operations;
using Vertizens.SliceR.Validated;

namespace Vertizens.SliceR.Minimal;
internal class UpdateEndpointValidatedHandlerRegistrar : IEndpointValidatedHandlerRegistrar
{
    public bool Handle(Type validatedHandlerInterface, ValidatedHandlerRegistrarContext context)
    {
        var handled = false;
        var arguments = validatedHandlerInterface.GetGenericArguments();
        if (arguments.Length == 2)
        {
            if (arguments[0].IsGenericType && arguments[0].GetGenericTypeDefinition() == typeof(Update<,>))
            {
                var updatedArguments = arguments[0].GetGenericArguments();
                var keyType = updatedArguments[0];
                var updateRequestType = updatedArguments[1];
                var resolveType = arguments[1];
                var resolved = HandlerResolvedContext.Create(resolveType, context.EntityDefinitionResolver, context.DomainToEntityTypeResolver);

                if (resolved.EntityDefinition != null && resolved.EntityDefinition.KeyType == keyType)
                {
                    if (resolved.DomainType != null)
                    {
                        context.Services.TryAddTransient(
                            validatedHandlerInterface,
                            typeof(UpdateValidatedHandler<,,,>).MakeGenericType(keyType, updateRequestType, resolved.DomainType, resolved.EntityDefinition.EntityType));
                    }
                    else
                    {
                        context.Services.TryAddTransient(
                            validatedHandlerInterface,
                            typeof(UpdateValidatedHandler<,,>).MakeGenericType(keyType, updateRequestType, resolved.EntityDefinition.EntityType));
                    }
                    handled = true;
                }
            }
        }

        return handled;
    }
}

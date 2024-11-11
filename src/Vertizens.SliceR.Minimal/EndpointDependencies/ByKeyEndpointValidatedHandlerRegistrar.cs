using Microsoft.Extensions.DependencyInjection.Extensions;
using Vertizens.SliceR.Operations;
using Vertizens.SliceR.Validated;

namespace Vertizens.SliceR.Minimal;
internal class ByKeyEndpointValidatedHandlerRegistrar : IEndpointValidatedHandlerRegistrar
{
    public bool Handle(Type validatedHandlerInterface, ValidatedHandlerRegistrarContext context)
    {
        var handled = false;
        var arguments = validatedHandlerInterface.GetGenericArguments();
        if (arguments.Length == 2)
        {
            var request = arguments[0];
            var result = arguments[1];
            if (request.IsGenericType && request.GetGenericTypeDefinition() == typeof(ByKey<>))
            {
                var argumentKeyType = request.GetGenericArguments()[0];
                var resolveType = result;
                var resolved = HandlerResolvedContext.Create(resolveType, context.EntityDefinitionResolver, context.DomainToEntityTypeResolver);

                if (resolved.EntityDefinition != null && resolved.EntityDefinition.KeyType == argumentKeyType)
                {
                    if (resolved.DomainType != null)
                    {
                        context.Services.TryAddTransient(
                            validatedHandlerInterface,
                            typeof(ByKeyValidatedHandler<,>).MakeGenericType(resolved.EntityDefinition.KeyType, resolved.DomainType));

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
                            validatedHandlerInterface,
                            typeof(ByKeyValidatedHandler<,>).MakeGenericType(resolved.EntityDefinition.KeyType, resolved.EntityDefinition.EntityType));
                    }
                    handled = true;
                }
            }
        }

        return handled;
    }
}

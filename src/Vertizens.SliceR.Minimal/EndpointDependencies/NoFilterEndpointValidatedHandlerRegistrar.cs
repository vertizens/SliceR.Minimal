using Microsoft.Extensions.DependencyInjection.Extensions;
using Vertizens.SliceR.Operations;
using Vertizens.SliceR.Validated;

namespace Vertizens.SliceR.Minimal;
internal class NoFilterEndpointValidatedHandlerRegistrar : IEndpointValidatedHandlerRegistrar
{
    public bool Handle(Type validatedHandlerInterface, ValidatedHandlerRegistrarContext context)
    {
        var handled = false;
        var arguments = validatedHandlerInterface.GetGenericArguments();
        if (arguments.Length == 2)
        {
            if (arguments[0] == typeof(NoFilter) && arguments[1].IsGenericType && arguments[1].GetGenericTypeDefinition() == typeof(IQueryable<>))
            {
                var resolveType = arguments[1].GetGenericArguments()[0];
                var resolved = HandlerResolvedTypes.Create(resolveType, context.EntityDefinitionResolver, context.DomainToEntityTypeResolver);

                if (resolved.EntityDefinition != null)
                {
                    context.Services.TryAddTransient(
                    validatedHandlerInterface,
                    typeof(NoFilterQueryableValidatedHandler<>).MakeGenericType(resolved.EntityDefinition.EntityType));
                    handled = true;
                }
            }
        }

        return handled;
    }
}

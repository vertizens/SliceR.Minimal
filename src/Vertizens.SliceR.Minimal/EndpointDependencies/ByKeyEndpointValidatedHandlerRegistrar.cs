﻿using Microsoft.Extensions.DependencyInjection.Extensions;
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
            if (arguments[0].IsGenericType && arguments[0].GetGenericTypeDefinition() == typeof(ByKey<>))
            {
                var argumentKeyType = arguments[0].GetGenericArguments()[0];
                var resolveType = arguments[1];
                var resolved = HandlerResolvedTypes.Create(resolveType, context.EntityDefinitionResolver, context.DomainToEntityTypeResolver);

                if (resolved.EntityDefinition != null && resolved.EntityDefinition.KeyType == argumentKeyType)
                {
                    if (resolved.DomainType != null)
                    {
                        context.Services.TryAddTransient(
                            validatedHandlerInterface,
                            typeof(ByKeyValidatedHandler<,,>).MakeGenericType(resolved.EntityDefinition.KeyType, resolved.EntityDefinition.EntityType, resolved.DomainType));
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
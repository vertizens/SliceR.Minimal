using Microsoft.AspNetCore.Http;
using Vertizens.SliceR.Operations;

namespace Vertizens.SliceR.Minimal;
internal class HandlerResolvedContext
{
    public EntityDefinition? EntityDefinition;
    public Type? DomainType;

    internal static HandlerResolvedContext Create(Type resolutionType, Endpoint endpoint, IEntityDefinitionResolver definitionResolver, IEntityMetadataTypeResolver entityTypeResolver)
    {
        Type? domainType = null;

        var entityDefinition = definitionResolver.Get(resolutionType);
        if (entityDefinition == null)
        {
            var endpointEntityType = entityTypeResolver.GetEntityType(endpoint);
            if (endpointEntityType != null)
            {
                domainType = resolutionType;
                entityDefinition = definitionResolver.Get(endpointEntityType);
            }

            if (entityDefinition == null)
            {
                var domainEntityType = entityTypeResolver.GetEntityType(resolutionType);
                if (domainEntityType != null)
                {
                    domainType = resolutionType;
                    entityDefinition = definitionResolver.Get(domainEntityType);
                }
            }
        }

        return new HandlerResolvedContext { EntityDefinition = entityDefinition, DomainType = domainType };
    }
}

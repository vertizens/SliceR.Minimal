using Vertizens.SliceR.Operations;

namespace Vertizens.SliceR.Minimal;
internal struct HandlerResolvedTypes
{
    public EntityDefinition? EntityDefinition;
    public Type? DomainType;

    internal static HandlerResolvedTypes Create(Type resolutionType, IEntityDefinitionResolver definitionResolver, IDomainToEntityTypeResolver entityTypeResolver)
    {
        Type? domainType = null;

        var entityDefinition = definitionResolver.Get(resolutionType);
        if (entityDefinition == null)
        {
            var domainToEntityType = entityTypeResolver.GetEntityType(resolutionType);
            if (domainToEntityType != null)
            {
                domainType = resolutionType;
                entityDefinition = definitionResolver.Get(domainToEntityType);
            }
        }

        return new HandlerResolvedTypes { EntityDefinition = entityDefinition, DomainType = domainType };
    }
}

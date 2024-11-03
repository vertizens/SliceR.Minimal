namespace Vertizens.SliceR.Minimal;
internal class DomainToEntityTypeResolver : IDomainToEntityTypeResolver
{
    public Type? GetEntityType(Type domainType)
    {
        Type? entityType = null;
        var domainMaps = domainType.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDomainToEntity<>)).ToList();
        if (domainMaps.Count == 1)
        {
            entityType = domainMaps[0].GetGenericArguments()[0];
        }
        return entityType;
    }
}

namespace Vertizens.SliceR.Minimal;

/// <summary>
/// Strategies that use a domain to entity resolution scheme implement this interface to help resolve default handler or validated handlers
/// </summary>
public interface IDomainToEntityTypeResolver
{
    /// <summary>
    /// Given a domain type, attempt to resolve the entity if applicable
    /// </summary>
    /// <param name="domainType"></param>
    /// <returns></returns>
    Type? GetEntityType(Type domainType);
}

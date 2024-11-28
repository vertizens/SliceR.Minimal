using Microsoft.AspNetCore.Http;

namespace Vertizens.SliceR.Minimal;

/// <summary>
/// Strategies that use a entity metadata resolution scheme implement this interface to help resolve default handler or validated handlers
/// </summary>
public interface IEntityMetadataTypeResolver
{
    /// <summary>
    /// Given a domain type, attempt to resolve the entity if applicable
    /// </summary>
    /// <param name="domainType"></param>
    /// <returns></returns>
    Type? GetEntityType(Type domainType);

    /// <summary>
    /// Given an endpoint, attempt to resolve the entity if applicable
    /// </summary>
    /// <param name="endpoint"></param>
    /// <returns></returns>
    Type? GetEntityType(Endpoint endpoint);
}

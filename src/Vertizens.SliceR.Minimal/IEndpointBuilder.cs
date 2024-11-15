using Microsoft.AspNetCore.Routing;

namespace Vertizens.SliceR.Minimal;

/// <summary>
/// Defines a class that will build endpoints for an API
/// </summary>
public interface IEndpointBuilder
{
    /// <summary>
    /// Builds endpoint routes 
    /// </summary>
    /// <param name="builder">Endpoint route builder</param>
    /// <returns></returns>
    IEndpointRouteBuilder Build(IEndpointRouteBuilder builder);
}

using Microsoft.AspNetCore.Routing;

namespace Vertizens.SliceR.Minimal;
public interface IEndpointBuilder
{
    IEndpointRouteBuilder Build(IEndpointRouteBuilder builder);
}

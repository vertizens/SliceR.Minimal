namespace Vertizens.SliceR.Minimal;
internal interface IEndpointHandlerDependencies
{
    IEnumerable<EndpointHandler> GetEndpointHandlers();
}

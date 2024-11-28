using Microsoft.AspNetCore.Http;

namespace Vertizens.SliceR.Minimal;
internal record class EndpointHandler(Endpoint Endpoint, Type HandlerType)
{

}

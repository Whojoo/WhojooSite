using Microsoft.AspNetCore.Routing;

namespace WhojooSite.Common.Api;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder);
}
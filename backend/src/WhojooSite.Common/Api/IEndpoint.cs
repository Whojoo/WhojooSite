using Microsoft.AspNetCore.Routing;

namespace WhojooSite.Common.Api;

public interface IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder);
}
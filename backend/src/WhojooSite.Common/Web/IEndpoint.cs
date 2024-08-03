using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace WhojooSite.Common.Web;

public interface IEndpoint
{
    void AddEndpoint(IEndpointRouteBuilder app);
}
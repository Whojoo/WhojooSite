using FastEndpoints;

using Microsoft.AspNetCore.Builder;

namespace WhojooSite.Fuel.Module.Endpoints;

public sealed class FuelModuleApiGroup : Group
{
    public FuelModuleApiGroup()
    {
        Configure(
            "/api/fuel-module",
            endpointDefinition =>
            {
                endpointDefinition.Options(builder => builder
                    .WithGroupName("fuel-module")
                    .WithOpenApi());
            });
    }
}
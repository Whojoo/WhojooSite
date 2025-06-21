using WhojooSite.View.Application.Interfaces;
using WhojooSite.View.Clients.FuelModule;

namespace WhojooSite.View.Infrastructure.Integrations.Fuel;

internal class HttpFuelModuleIntegration(FuelModuleClient fuelModuleClient) : IFuelModule
{
    private readonly FuelModuleClient _fuelModuleClient = fuelModuleClient;

    public async Task<List<Guid>> GetTrackableObjectsAsync()
    {
        var foo = await _fuelModuleClient.Api.FuelModule.TrackableObjects.GetAsync();

        return foo?.TrackableObjects is null
            ? []
            : foo.TrackableObjects.Where(x => x.Id.HasValue).Select(x => x.Id!.Value).ToList();
    }
}
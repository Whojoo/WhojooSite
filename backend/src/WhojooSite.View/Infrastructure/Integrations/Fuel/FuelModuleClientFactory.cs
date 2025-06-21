using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;

using WhojooSite.View.Clients.FuelModule;

namespace WhojooSite.View.Infrastructure.Integrations.Fuel;

internal class FuelModuleClientFactory(HttpClient httpClient)
{
    private readonly IAuthenticationProvider _authenticationProvider = new AnonymousAuthenticationProvider();
    private readonly HttpClient _httpClient = httpClient;

    public FuelModuleClient GetClient()
    {
        return new FuelModuleClient(new HttpClientRequestAdapter(
            _authenticationProvider,
            httpClient: _httpClient));
    }
}
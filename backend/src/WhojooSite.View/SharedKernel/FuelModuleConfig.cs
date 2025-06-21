using WhojooSite.Common.Config;

namespace WhojooSite.View.SharedKernel;

public class FuelModuleConfig : IConfig
{
    public Uri BaseUrl { get; set; } = null!;
    public string Position => "FuelModule";
}
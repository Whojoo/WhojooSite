using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WhojooSite.Common;

public interface ISettings
{
    string Position { get; }
}

public static class SettingsExtensions
{
    public static IServiceCollection AddSetting<TSettingModel>(this IServiceCollection services, IConfiguration configuration)
        where TSettingModel : class, ISettings
    {
        services.AddOptions<TSettingModel>()
            .Configure(settingsModel => configuration.Bind(settingsModel.Position, settingsModel));

        return services;
    }
}
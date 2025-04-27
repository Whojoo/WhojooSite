using WhojooSite.Common;

namespace WhojooSite.View.Application.Recipes;

public class RecipesModuleSettings : ISettings
{
    public string GrpcUrl { get; init; } = null!;
    public string Position => "RecipesModule";
}
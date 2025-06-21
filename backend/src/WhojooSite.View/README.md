# Recipes client

dotnet kiota generate -l CSharp -c RecipeModuleClient -n WhojooSite.View.Clients.RecipesModule -d
OpenApi\WhojooSite.Bootstrap_recipes-module.json -o ./Clients/RecipesModule

# Fuel client

dotnet kiota generate -l CSharp -c FuelModuleClient -n WhojooSite.View.Clients.FuelModule -d
OpenApi\WhojooSite.Bootstrap_fuel-module.json -o ./Clients/FuelModule

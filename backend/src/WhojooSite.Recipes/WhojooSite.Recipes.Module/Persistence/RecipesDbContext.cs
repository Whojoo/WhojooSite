using Microsoft.EntityFrameworkCore;

using WhojooSite.Recipes.Module.Domain.Cookbook;
using WhojooSite.Recipes.Module.Domain.Recipes;
using WhojooSite.Recipes.Module.Domain.SpiceMix;

namespace WhojooSite.Recipes.Module.Persistence;

public class RecipesDbContext(DbContextOptions<RecipesDbContext> options) : DbContext(options)
{
    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<Cookbook> Cookbooks { get; set; }

    public virtual DbSet<SpiceMix> SpiceMixes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IRecipesModuleAssemblyMarker).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
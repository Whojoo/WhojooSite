using Microsoft.EntityFrameworkCore;

using WhojooSite.Recipes.Module.Domain.Cookbooks;
using WhojooSite.Recipes.Module.Domain.Recipes;
using WhojooSite.Recipes.Module.Domain.SpiceMix;

namespace WhojooSite.Recipes.Module.Infrastructure.Persistence;

internal class RecipesDbContext(DbContextOptions<RecipesDbContext> options) : DbContext(options)
{
    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<Cookbook> Cookbooks { get; set; }

    public virtual DbSet<SpiceMix> SpiceMixes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RecipesModuleInitializer).Assembly);

        modelBuilder.HasDefaultSchema("recipes");

        base.OnModelCreating(modelBuilder);
    }
}
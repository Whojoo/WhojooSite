using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhojooSite.Recipes.Module.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "recipes");

            migrationBuilder.RenameTable(
                name: "Steps",
                newName: "Steps",
                newSchema: "recipes");

            migrationBuilder.RenameTable(
                name: "SpiceMixIngredients",
                newName: "SpiceMixIngredients",
                newSchema: "recipes");

            migrationBuilder.RenameTable(
                name: "SpiceMixes",
                newName: "SpiceMixes",
                newSchema: "recipes");

            migrationBuilder.RenameTable(
                name: "RecipeSpices",
                newName: "RecipeSpices",
                newSchema: "recipes");

            migrationBuilder.RenameTable(
                name: "RecipeSpiceMixIngredients",
                newName: "RecipeSpiceMixIngredients",
                newSchema: "recipes");

            migrationBuilder.RenameTable(
                name: "Recipes",
                newName: "Recipes",
                newSchema: "recipes");

            migrationBuilder.RenameTable(
                name: "RecipeIngredients",
                newName: "RecipeIngredients",
                newSchema: "recipes");

            migrationBuilder.RenameTable(
                name: "Cookbooks",
                newName: "Cookbooks",
                newSchema: "recipes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Steps",
                schema: "recipes",
                newName: "Steps");

            migrationBuilder.RenameTable(
                name: "SpiceMixIngredients",
                schema: "recipes",
                newName: "SpiceMixIngredients");

            migrationBuilder.RenameTable(
                name: "SpiceMixes",
                schema: "recipes",
                newName: "SpiceMixes");

            migrationBuilder.RenameTable(
                name: "RecipeSpices",
                schema: "recipes",
                newName: "RecipeSpices");

            migrationBuilder.RenameTable(
                name: "RecipeSpiceMixIngredients",
                schema: "recipes",
                newName: "RecipeSpiceMixIngredients");

            migrationBuilder.RenameTable(
                name: "Recipes",
                schema: "recipes",
                newName: "Recipes");

            migrationBuilder.RenameTable(
                name: "RecipeIngredients",
                schema: "recipes",
                newName: "RecipeIngredients");

            migrationBuilder.RenameTable(
                name: "Cookbooks",
                schema: "recipes",
                newName: "Cookbooks");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeMeal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDbSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Nutritions_RecipeId",
                table: "Nutritions");

            migrationBuilder.CreateIndex(
                name: "IX_Nutritions_RecipeId",
                table: "Nutritions",
                column: "RecipeId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Nutritions_RecipeId",
                table: "Nutritions");

            migrationBuilder.CreateIndex(
                name: "IX_Nutritions_RecipeId",
                table: "Nutritions",
                column: "RecipeId");
        }
    }
}

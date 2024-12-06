using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeMeal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIsPurchasedFromShoppingListItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPurchased",
                table: "ShoppingListItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPurchased",
                table: "ShoppingListItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}

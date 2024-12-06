using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeMeal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixUserShoppingListItemRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "UserShoppingListItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserShoppingListItems");
        }
    }
}

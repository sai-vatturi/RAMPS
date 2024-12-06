using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeMeal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateShoppingListConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserShoppingListItems_UserShoppingLists_UserShoppingListId",
                table: "UserShoppingListItems");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserShoppingListItems");

            migrationBuilder.AddForeignKey(
                name: "FK_UserShoppingListItems_UserShoppingLists_UserShoppingListId",
                table: "UserShoppingListItems",
                column: "UserShoppingListId",
                principalTable: "UserShoppingLists",
                principalColumn: "UserShoppingListId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserShoppingListItems_UserShoppingLists_UserShoppingListId",
                table: "UserShoppingListItems");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "UserShoppingListItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_UserShoppingListItems_UserShoppingLists_UserShoppingListId",
                table: "UserShoppingListItems",
                column: "UserShoppingListId",
                principalTable: "UserShoppingLists",
                principalColumn: "UserShoppingListId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

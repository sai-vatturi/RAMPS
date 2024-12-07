using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeMeal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class newmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserShoppingListItems_UserShoppingLists_UserShoppingListId",
                table: "UserShoppingListItems");

            migrationBuilder.AddForeignKey(
                name: "FK_UserShoppingListItems_UserShoppingLists_UserShoppingListId",
                table: "UserShoppingListItems",
                column: "UserShoppingListId",
                principalTable: "UserShoppingLists",
                principalColumn: "UserShoppingListId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserShoppingListItems_UserShoppingLists_UserShoppingListId",
                table: "UserShoppingListItems");

            migrationBuilder.AddForeignKey(
                name: "FK_UserShoppingListItems_UserShoppingLists_UserShoppingListId",
                table: "UserShoppingListItems",
                column: "UserShoppingListId",
                principalTable: "UserShoppingLists",
                principalColumn: "UserShoppingListId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

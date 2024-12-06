using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeMeal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserShoppingListItems_UserShoppingLists_UserShoppingListId",
                table: "UserShoppingListItems");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "UserShoppingLists",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserShoppingLists",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

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

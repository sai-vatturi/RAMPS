using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeMeal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SimplifiedShoppingListModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserShoppingListItems_ShoppingListItems_ShoppingListItemId",
                table: "UserShoppingListItems");

            migrationBuilder.DropTable(
                name: "ShoppingListItems");

            migrationBuilder.DropTable(
                name: "ShoppingLists");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserShoppingListItems");

            migrationBuilder.RenameColumn(
                name: "ShoppingListItemId",
                table: "UserShoppingListItems",
                newName: "UserShoppingListId");

            migrationBuilder.RenameIndex(
                name: "IX_UserShoppingListItems_ShoppingListItemId",
                table: "UserShoppingListItems",
                newName: "IX_UserShoppingListItems_UserShoppingListId");

            migrationBuilder.AddColumn<string>(
                name: "Ingredient",
                table: "UserShoppingListItems",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "UserShoppingListItems",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Reviews",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.CreateTable(
                name: "UserShoppingLists",
                columns: table => new
                {
                    UserShoppingListId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MealPlanId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserShoppingLists", x => x.UserShoppingListId);
                });

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

            migrationBuilder.DropTable(
                name: "UserShoppingLists");

            migrationBuilder.DropColumn(
                name: "Ingredient",
                table: "UserShoppingListItems");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "UserShoppingListItems");

            migrationBuilder.RenameColumn(
                name: "UserShoppingListId",
                table: "UserShoppingListItems",
                newName: "ShoppingListItemId");

            migrationBuilder.RenameIndex(
                name: "IX_UserShoppingListItems_UserShoppingListId",
                table: "UserShoppingListItems",
                newName: "IX_UserShoppingListItems_ShoppingListItemId");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "UserShoppingListItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Reviews",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateTable(
                name: "ShoppingLists",
                columns: table => new
                {
                    ShoppingListId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MealPlanId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingLists", x => x.ShoppingListId);
                    table.ForeignKey(
                        name: "FK_ShoppingLists_MealPlans_MealPlanId",
                        column: x => x.MealPlanId,
                        principalTable: "MealPlans",
                        principalColumn: "MealPlanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingListItems",
                columns: table => new
                {
                    ShoppingListItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShoppingListId = table.Column<int>(type: "int", nullable: false),
                    Ingredient = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingListItems", x => x.ShoppingListItemId);
                    table.ForeignKey(
                        name: "FK_ShoppingListItems_ShoppingLists_ShoppingListId",
                        column: x => x.ShoppingListId,
                        principalTable: "ShoppingLists",
                        principalColumn: "ShoppingListId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingListItems_ShoppingListId",
                table: "ShoppingListItems",
                column: "ShoppingListId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingLists_MealPlanId",
                table: "ShoppingLists",
                column: "MealPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserShoppingListItems_ShoppingListItems_ShoppingListItemId",
                table: "UserShoppingListItems",
                column: "ShoppingListItemId",
                principalTable: "ShoppingListItems",
                principalColumn: "ShoppingListItemId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

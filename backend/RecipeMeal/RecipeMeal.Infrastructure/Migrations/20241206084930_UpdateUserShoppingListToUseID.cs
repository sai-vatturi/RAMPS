using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeMeal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserShoppingListToUseID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserShoppingListItems_UserShoppingLists_UserShoppingListId1",
                table: "UserShoppingListItems");

            migrationBuilder.DropForeignKey(
                name: "FK_UserShoppingLists_MealPlans_MealPlanId",
                table: "UserShoppingLists");

            migrationBuilder.DropIndex(
                name: "IX_UserShoppingLists_MealPlanId",
                table: "UserShoppingLists");

            migrationBuilder.DropIndex(
                name: "IX_UserShoppingListItems_UserShoppingListId1",
                table: "UserShoppingListItems");

            migrationBuilder.DropColumn(
                name: "UserShoppingListId1",
                table: "UserShoppingListItems");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "UserShoppingLists",
                newName: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserShoppingLists",
                newName: "Username");

            migrationBuilder.AddColumn<int>(
                name: "UserShoppingListId1",
                table: "UserShoppingListItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserShoppingLists_MealPlanId",
                table: "UserShoppingLists",
                column: "MealPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_UserShoppingListItems_UserShoppingListId1",
                table: "UserShoppingListItems",
                column: "UserShoppingListId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserShoppingListItems_UserShoppingLists_UserShoppingListId1",
                table: "UserShoppingListItems",
                column: "UserShoppingListId1",
                principalTable: "UserShoppingLists",
                principalColumn: "UserShoppingListId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserShoppingLists_MealPlans_MealPlanId",
                table: "UserShoppingLists",
                column: "MealPlanId",
                principalTable: "MealPlans",
                principalColumn: "MealPlanId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

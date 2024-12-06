using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeMeal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserShoppingListItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserShoppingListItems",
                columns: table => new
                {
                    UserShoppingListItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShoppingListItemId = table.Column<int>(type: "int", nullable: false),
                    IsPurchased = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserShoppingListItems", x => x.UserShoppingListItemId);
                    table.ForeignKey(
                        name: "FK_UserShoppingListItems_ShoppingListItems_ShoppingListItemId",
                        column: x => x.ShoppingListItemId,
                        principalTable: "ShoppingListItems",
                        principalColumn: "ShoppingListItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserShoppingListItems_ShoppingListItemId",
                table: "UserShoppingListItems",
                column: "ShoppingListItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserShoppingListItems");
        }
    }
}

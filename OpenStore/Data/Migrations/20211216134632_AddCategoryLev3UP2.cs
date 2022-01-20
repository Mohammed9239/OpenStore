using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenStore.Data.Migrations
{
    public partial class AddCategoryLev3UP2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_CategoryLev3s_CategoryLev3ID",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_CategoryLev3ID",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CategoryLev3ID",
                table: "Products");

            migrationBuilder.CreateIndex(
                name: "IX_Products_GetCategoryLev3ID",
                table: "Products",
                column: "GetCategoryLev3ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_CategoryLev3s_GetCategoryLev3ID",
                table: "Products",
                column: "GetCategoryLev3ID",
                principalTable: "CategoryLev3s",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_CategoryLev3s_GetCategoryLev3ID",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_GetCategoryLev3ID",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "CategoryLev3ID",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryLev3ID",
                table: "Products",
                column: "CategoryLev3ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_CategoryLev3s_CategoryLev3ID",
                table: "Products",
                column: "CategoryLev3ID",
                principalTable: "CategoryLev3s",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

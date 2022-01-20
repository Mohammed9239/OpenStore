using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenStore.Data.Migrations
{
    public partial class AddCategoryLev3Database : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryLev3_CategoryLev2s_CategoryLev2ID",
                table: "CategoryLev3");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_CategoryLev3_CategoryLev3ID",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_PropertyLev1s_CategoryLev3_CategoryLev3ID",
                table: "PropertyLev1s");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryLev3",
                table: "CategoryLev3");

            migrationBuilder.RenameTable(
                name: "CategoryLev3",
                newName: "CategoryLev3s");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryLev3_CategoryLev2ID",
                table: "CategoryLev3s",
                newName: "IX_CategoryLev3s_CategoryLev2ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryLev3s",
                table: "CategoryLev3s",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryLev3s_CategoryLev2s_CategoryLev2ID",
                table: "CategoryLev3s",
                column: "CategoryLev2ID",
                principalTable: "CategoryLev2s",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_CategoryLev3s_CategoryLev3ID",
                table: "Products",
                column: "CategoryLev3ID",
                principalTable: "CategoryLev3s",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyLev1s_CategoryLev3s_CategoryLev3ID",
                table: "PropertyLev1s",
                column: "CategoryLev3ID",
                principalTable: "CategoryLev3s",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryLev3s_CategoryLev2s_CategoryLev2ID",
                table: "CategoryLev3s");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_CategoryLev3s_CategoryLev3ID",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_PropertyLev1s_CategoryLev3s_CategoryLev3ID",
                table: "PropertyLev1s");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryLev3s",
                table: "CategoryLev3s");

            migrationBuilder.RenameTable(
                name: "CategoryLev3s",
                newName: "CategoryLev3");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryLev3s_CategoryLev2ID",
                table: "CategoryLev3",
                newName: "IX_CategoryLev3_CategoryLev2ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryLev3",
                table: "CategoryLev3",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryLev3_CategoryLev2s_CategoryLev2ID",
                table: "CategoryLev3",
                column: "CategoryLev2ID",
                principalTable: "CategoryLev2s",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_CategoryLev3_CategoryLev3ID",
                table: "Products",
                column: "CategoryLev3ID",
                principalTable: "CategoryLev3",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyLev1s_CategoryLev3_CategoryLev3ID",
                table: "PropertyLev1s",
                column: "CategoryLev3ID",
                principalTable: "CategoryLev3",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

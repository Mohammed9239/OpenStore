using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenStore.Data.Migrations
{
    public partial class AddCategoryLev3UP1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyLev1s_CategoryLev3s_CategoryLev3ID",
                table: "PropertyLev1s");

            migrationBuilder.DropIndex(
                name: "IX_PropertyLev1s_CategoryLev3ID",
                table: "PropertyLev1s");

            migrationBuilder.DropColumn(
                name: "CategoryLev3ID",
                table: "PropertyLev1s");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryLev3ID",
                table: "PropertyLev1s",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PropertyLev1s_CategoryLev3ID",
                table: "PropertyLev1s",
                column: "CategoryLev3ID");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyLev1s_CategoryLev3s_CategoryLev3ID",
                table: "PropertyLev1s",
                column: "CategoryLev3ID",
                principalTable: "CategoryLev3s",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

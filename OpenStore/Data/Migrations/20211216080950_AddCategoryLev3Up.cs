using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenStore.Data.Migrations
{
    public partial class AddCategoryLev3Up : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GetCategoryLev3ID",
                table: "Products",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GetCategoryLev3ID",
                table: "Products");
        }
    }
}

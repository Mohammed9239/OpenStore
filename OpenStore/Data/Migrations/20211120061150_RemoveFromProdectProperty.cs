using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenStore.Data.Migrations
{
    public partial class RemoveFromProdectProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "ProdectProperties");

            migrationBuilder.DropColumn(
                name: "Qunatity",
                table: "ProdectProperties");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "ProdectProperties",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Qunatity",
                table: "ProdectProperties",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

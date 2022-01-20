using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenStore.Data.Migrations
{
    public partial class AddCategoryLev3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryLev3ID",
                table: "PropertyLev1s",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryLev3ID",
                table: "Products",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CategoryLev3",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    ImgUrl = table.Column<string>(nullable: true),
                    Index = table.Column<int>(nullable: false),
                    CategoryLev2ID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryLev3", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CategoryLev3_CategoryLev2s_CategoryLev2ID",
                        column: x => x.CategoryLev2ID,
                        principalTable: "CategoryLev2s",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PropertyLev1s_CategoryLev3ID",
                table: "PropertyLev1s",
                column: "CategoryLev3ID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryLev3ID",
                table: "Products",
                column: "CategoryLev3ID");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryLev3_CategoryLev2ID",
                table: "CategoryLev3",
                column: "CategoryLev2ID");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_CategoryLev3_CategoryLev3ID",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_PropertyLev1s_CategoryLev3_CategoryLev3ID",
                table: "PropertyLev1s");

            migrationBuilder.DropTable(
                name: "CategoryLev3");

            migrationBuilder.DropIndex(
                name: "IX_PropertyLev1s_CategoryLev3ID",
                table: "PropertyLev1s");

            migrationBuilder.DropIndex(
                name: "IX_Products_CategoryLev3ID",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CategoryLev3ID",
                table: "PropertyLev1s");

            migrationBuilder.DropColumn(
                name: "CategoryLev3ID",
                table: "Products");
        }
    }
}

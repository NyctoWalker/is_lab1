using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace is_lab6.Migrations
{
    public partial class AddDictionary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TypeCode",
                table: "shop_items",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Types",
                columns: table => new
                {
                    Code = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TypeName = table.Column<string>(type: "varchar(30)", nullable: false, collation: "utf8mb3_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb3")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Types", x => x.Code);
                })
                .Annotation("MySql:CharSet", "utf8mb3")
                .Annotation("Relational:Collation", "utf8mb3_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_shop_items_TypeCode",
                table: "shop_items",
                column: "TypeCode");

            migrationBuilder.AddForeignKey(
                name: "FK_shop_items_Types_TypeCode",
                table: "shop_items",
                column: "TypeCode",
                principalTable: "Types",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_shop_items_Types_TypeCode",
                table: "shop_items");

            migrationBuilder.DropTable(
                name: "Types");

            migrationBuilder.DropIndex(
                name: "IX_shop_items_TypeCode",
                table: "shop_items");

            migrationBuilder.DropColumn(
                name: "TypeCode",
                table: "shop_items");
        }
    }
}

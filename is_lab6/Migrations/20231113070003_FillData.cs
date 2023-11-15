using Microsoft.EntityFrameworkCore.Migrations;

namespace is_lab6.Migrations
{
    public partial class FillData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                    table: "Types",
                    columns: new[] { "Code", "TypeName" },
                    values: new object[,] {
                        { 1, "Телефон" },
                        { 2, "Игровая приставка" },
                        { 3, "Телевизор" },
                        { 4, "Часы" },
                    }
                );

            migrationBuilder.InsertData(
                    table: "shop_items",
                    columns: new[] { "item_name", "item_price", "ItemColor", "ItemWeight", "ItemLifetime", "TypeCode" },
                    values: new object[,] {
                        { "Телефон ASUS", 30000, "Чёрный", "200г", "2г", 1 },
                        { "Телевизор SONY", 59990, "Синий", "100 кг", "1 г.", 3},
                    }
                );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

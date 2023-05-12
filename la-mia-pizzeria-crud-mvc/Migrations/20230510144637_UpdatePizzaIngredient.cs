using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace la_mia_pizzeria_crud_mvc.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePizzaIngredient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PizzaIngredient",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PizzaIngredient", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PizzaIngredientPizzas",
                columns: table => new
                {
                    PizzaIngredientId = table.Column<int>(type: "int", nullable: false),
                    PizzasId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PizzaIngredientPizzas", x => new { x.PizzaIngredientId, x.PizzasId });
                    table.ForeignKey(
                        name: "FK_PizzaIngredientPizzas_PizzaIngredient_PizzaIngredientId",
                        column: x => x.PizzaIngredientId,
                        principalTable: "PizzaIngredient",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PizzaIngredientPizzas_pizzas_PizzasId",
                        column: x => x.PizzasId,
                        principalTable: "pizzas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PizzaIngredientPizzas_PizzasId",
                table: "PizzaIngredientPizzas",
                column: "PizzasId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PizzaIngredientPizzas");

            migrationBuilder.DropTable(
                name: "PizzaIngredient");
        }
    }
}

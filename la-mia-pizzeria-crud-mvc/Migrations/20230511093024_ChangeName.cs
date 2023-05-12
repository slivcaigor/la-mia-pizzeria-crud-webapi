using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace la_mia_pizzeria_crud_mvc.Migrations
{
    /// <inheritdoc />
    public partial class ChangeName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IngredientPizzas");

            migrationBuilder.DropTable(
                name: "pizzas");

            migrationBuilder.CreateTable(
                name: "pizza",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    price = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pizza", x => x.id);
                    table.ForeignKey(
                        name: "FK_pizza_categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "categories",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "IngredientPizza",
                columns: table => new
                {
                    IngredientsId = table.Column<int>(type: "int", nullable: false),
                    PizzasId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngredientPizza", x => new { x.IngredientsId, x.PizzasId });
                    table.ForeignKey(
                        name: "FK_IngredientPizza_ingredients_IngredientsId",
                        column: x => x.IngredientsId,
                        principalTable: "ingredients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IngredientPizza_pizza_PizzasId",
                        column: x => x.PizzasId,
                        principalTable: "pizza",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IngredientPizza_PizzasId",
                table: "IngredientPizza",
                column: "PizzasId");

            migrationBuilder.CreateIndex(
                name: "IX_pizza_CategoryId",
                table: "pizza",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IngredientPizza");

            migrationBuilder.DropTable(
                name: "pizza");

            migrationBuilder.CreateTable(
                name: "pizzas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    price = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pizzas", x => x.id);
                    table.ForeignKey(
                        name: "FK_pizzas_categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "categories",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "IngredientPizzas",
                columns: table => new
                {
                    IngredientsId = table.Column<int>(type: "int", nullable: false),
                    PizzasId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngredientPizzas", x => new { x.IngredientsId, x.PizzasId });
                    table.ForeignKey(
                        name: "FK_IngredientPizzas_ingredients_IngredientsId",
                        column: x => x.IngredientsId,
                        principalTable: "ingredients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IngredientPizzas_pizzas_PizzasId",
                        column: x => x.PizzasId,
                        principalTable: "pizzas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IngredientPizzas_PizzasId",
                table: "IngredientPizzas",
                column: "PizzasId");

            migrationBuilder.CreateIndex(
                name: "IX_pizzas_CategoryId",
                table: "pizzas",
                column: "CategoryId");
        }
    }
}

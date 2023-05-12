using la_mia_pizzeria_crud_mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace la_mia_pizzeria_crud_mvc.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PizzasController : ControllerBase
    {
        // Inizio del metodo per ottenere la lista di tutte le pizze + filtro per nome
        [HttpGet]
        public IActionResult GetPizzas(string? search)
        {
            using var db = new PizzaContext();
            var pizzas = db.Pizza.Include(p => p.Category).Include(p => p.Ingredients).AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                pizzas = pizzas.Where(p => p.Name.Contains(search));
            }

            return Ok(pizzas.ToList());
        }

        // Inizio del metodo per ottenere la pizza in base all'id
        [HttpGet("{id}")]
        public IActionResult GetPizzaDetails(int id)
        {
            using PizzaContext db = new();
            Pizza? pizza = db.Pizza
                .Where(p => p.Id == id)
                .Include(p => p.Category)
                .Include(p => p.Ingredients)
                .FirstOrDefault();

            if (pizza != null)
            {
                return Ok(pizza);
            }

            return NotFound();
        }

        // Inizio del metodo per creare una pizza 
        [HttpPost]
        public IActionResult Create([FromBody] PizzaFormModel data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using PizzaContext db = new();
            Pizza pizza = new()
            {
                Name = data.Pizza.Name,
                Description = data.Pizza.Description,
                Price = data.Pizza.Price,
                Image = data.Pizza.Image,
                CategoryId = data.Pizza.CategoryId,
                Ingredients = new List<Ingredient>()
            };

            if (data.SelectedIngredients != null)
            {
                foreach (string? selectedIngredientId in data.SelectedIngredients)
                {
                    if (selectedIngredientId != null && int.TryParse(selectedIngredientId, out int ingredientId))
                    {
                        Ingredient? ingredient = db.Ingredients.FirstOrDefault(ing => ing.Id == ingredientId);
                        if (ingredient != null)
                        {
                            pizza.Ingredients.Add(ingredient);
                        }
                    }
                }
            }

            db.Pizza.Add(pizza);
            db.SaveChanges();

            return CreatedAtAction(nameof(GetPizzaDetails), new { id = pizza.Id }, pizza);
        }


        // Inizio del metodo per modificare una pizza 
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] PizzaFormModel data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using PizzaContext db = new();
            Pizza? pizza = db.Pizza
                .Where(p => p.Id == id)
                .Include(p => p.Ingredients)
                .FirstOrDefault();

            if (pizza == null)
            {
                return NotFound();
            }

            pizza.Name = data.Pizza.Name;
            pizza.Description = data.Pizza.Description;
            pizza.Price = data.Pizza.Price;
            pizza.CategoryId = data.Pizza.CategoryId;

            if (pizza.Ingredients == null)
            {
                pizza.Ingredients = new List<Ingredient>();
            }
            else
            {
                pizza.Ingredients.Clear();
            }

            if (data.SelectedIngredients != null)
            {
                foreach (string? selectedIngredientId in data.SelectedIngredients)
                {
                    if (selectedIngredientId is not null && int.TryParse(selectedIngredientId, out int ingredientId))
                    {
                        Ingredient? ingredient = db.Ingredients.FirstOrDefault(ing => ing.Id == ingredientId);
                        if (ingredient is not null)
                        {
                            pizza.Ingredients.Add(ingredient);
                        }
                    }
                }
            }

            db.SaveChanges();

            return Ok(pizza);
        }



        // Inizio del metodo per eliminare una pizza 
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using PizzaContext db = new();
            Pizza? pizzaToDelete = db.Pizza
                .Where(pizza => pizza.Id == id).FirstOrDefault();
            if (pizzaToDelete != null)
            {
                db.Pizza.Remove(pizzaToDelete);
                db.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }


    }
}


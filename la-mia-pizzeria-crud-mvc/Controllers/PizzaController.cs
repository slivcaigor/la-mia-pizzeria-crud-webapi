using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using la_mia_pizzeria_crud_mvc.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace la_mia_pizzeria_crud_mvc.Controllers
{
    public class PizzaController : Controller
    {
        [HttpGet]
        public IActionResult Index(int pageNumber = 1, int pageSize = 6)
        {
            using PizzaContext db = new();
            List<Pizza> pizzas = db.Pizza
                .OrderBy(pizza => pizza.Id)
                .Include(pizza => pizza.Category)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            if (pizzas.Count == 0)
            {
                ViewBag.Message = "We have eaten all the pizzas!";
            }

            int totalPizzas = db.Pizza.Count();
            ViewData["TotalPizzas"] = totalPizzas;
            ViewBag.PageSize = pageSize;
            ViewBag.CurrentPage = pageNumber;

            return View("Index", pizzas);
        }


        [HttpGet]
        public IActionResult Details(int id)
        {
            using PizzaContext db = new();
            Pizza? pizza = db.Pizza
                .Where(pizza => pizza.Id == id)
                .Include(pizza => pizza.Category)
                .Include(pizza => pizza.Ingredients)
                .FirstOrDefault();

            if (pizza != null)
            {
                return View("Details", pizza);
            }
                return RedirectToAction("Error404");
            
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            using PizzaContext db = new();
            List<Category> categories = db.Categories.ToList();
            PizzaFormModel model = new();
            List<Ingredient> ingredients = db.Ingredients.ToList();

            List<SelectListItem> listIngredients = new();

            foreach (Ingredient ingredient in ingredients)
            {
                listIngredients.Add(new SelectListItem()
                { Text = ingredient.Name, Value = ingredient.Id.ToString() });
            }


            model.Pizza = new Pizza();
            model.Categories = categories;
            model.Ingredient = listIngredients;

            return View("Create", model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PizzaFormModel data)
        {
            if (!ModelState.IsValid)
            {
                using PizzaContext context = new();
                List<Category> categories = context.Categories.ToList();
                data.Categories = categories;

                List<Ingredient> ingredients = context.Ingredients.ToList();
                List<SelectListItem> listIngredients = new();

                foreach (Ingredient ingredient in ingredients)
                {
                    listIngredients.Add(new SelectListItem()
                    { Text = ingredient.Name, Value = ingredient.Id.ToString() });
                }

                data.Ingredient = listIngredients;

                return View(data);
            }

            using PizzaContext db = new();
            Pizza? pizza = new()
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

            return RedirectToAction("Index");
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Update(int id)
        {
            using PizzaContext db = new();
            Pizza? pizza = db.Pizza
                .Include(p => p.Ingredients)
                .FirstOrDefault(p => p.Id == id);

            if (pizza == null)
            {
                return NotFound();
            }

            List<Category> categories = db.Categories.ToList();
            List<Ingredient> ingredients = db.Ingredients.ToList();
            List<SelectListItem> listIngredients = new();

            foreach (Ingredient ingredient in ingredients)
            {
                bool isSelected = pizza.Ingredients != null && pizza.Ingredients.Any(i => i.Id == ingredient.Id);
                listIngredients.Add(new SelectListItem
                {
                    Text = ingredient.Name,
                    Value = ingredient.Id.ToString(),
                    Selected = isSelected
                });
            }

            PizzaFormModel model = new()
            {
                Pizza = pizza,
                Categories = categories,
                Ingredient = listIngredients,
                SelectedIngredients = pizza.Ingredients?.Select(i => i.Id.ToString()).ToList()
            };

            return View(model);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, PizzaFormModel data)
        {
            if (!ModelState.IsValid)
            {
                using PizzaContext context = new();
                List<Category> categories = context.Categories.ToList();
                List<Ingredient> ingredients = context.Ingredients.ToList();
                List<SelectListItem> listIngredients = new();

                foreach (Ingredient ingredient in ingredients)
                {
                    listIngredients.Add(new SelectListItem()
                    {
                        Text = ingredient.Name,
                        Value = ingredient.Id.ToString(),
                        Selected = data.SelectedIngredients?.Contains(ingredient.Id.ToString()) ?? false
                    });
                }

                data.Categories = categories;
                data.Ingredient = listIngredients;

                return View("Update", data);
            }

            using PizzaContext db = new();
            Pizza? pizza = db.Pizza
                .Where(p => p.Id == id)
                .Include(p => p.Ingredients)
                .FirstOrDefault();

            if (pizza != null)
            {
                pizza.Name = data.Pizza.Name;
                pizza.Description = data.Pizza.Description;
                pizza.Price = data.Pizza.Price;
                pizza.CategoryId = data.Pizza.CategoryId;
                pizza.Ingredients?.Clear();

                if (data.SelectedIngredients != null)
                {
                    foreach (string? selectedIngredientId in data.SelectedIngredients)
                    {
                        if (selectedIngredientId is not null && int.TryParse(selectedIngredientId, out int ingredientId))
                        {
                            Ingredient? ingredient = db.Ingredients.FirstOrDefault(ing => ing.Id == ingredientId);
                            if (ingredient is not null)
                            {
                                pizza.Ingredients?.Add(ingredient);
                            }
                        }
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Error404");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            using PizzaContext db = new();
            Pizza? pizza = db.Pizza.Where(pizza => pizza.Id == id).FirstOrDefault();

            if (pizza != null)
            {
                db.Pizza.Remove(pizza);

                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Error404");
            }
        }
    }
}


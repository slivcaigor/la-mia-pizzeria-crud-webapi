﻿using Microsoft.AspNetCore.Mvc.Rendering;

namespace la_mia_pizzeria_crud_mvc.Models
{
    public class PizzaFormModel
    {
        public Pizza? Pizza { get; set; }
        public List<Category>? Categories { get; set; }
        public List<SelectListItem>? Ingredient { get; set; }
        public List<string>? SelectedIngredients { get; set; }
    }
}

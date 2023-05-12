using la_mia_pizzeria_crud_mvc.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace la_mia_pizzeria_crud_mvc
{
    public class PizzaContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Pizza> Pizza { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=pizzadex;Integrated Security=True;TrustServerCertificate=True");
        }
    }
}

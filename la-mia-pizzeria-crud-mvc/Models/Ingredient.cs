using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace la_mia_pizzeria_crud_mvc.Models
{
    [Table("ingredients")]
    public class Ingredient
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        [MaxLength(32)]
        public string? Name { get; set; }
        public List<Pizza> Pizzas { get; set; }
    }
}
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace la_mia_pizzeria_crud_mvc.Models
{
    [Table("categories")]
    public class Category
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        [MaxLength(32)]
        public string? Name { get; set; }

        public List<Pizza>? Pizzas { get; set; }
    }
}
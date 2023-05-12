using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace la_mia_pizzeria_crud_mvc.Models
{
    [Table("pizza")]
    public class Pizza
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Il nome è obbligatorio")]
        [Column("name")]
        [MaxLength(32)]
        public string? Name { get; set; }

        [Required(ErrorMessage = "La descrizione è obbligatoria")]
        [Column("description")]
        [MaxLength(500)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Il link all'immagine è obbligatorio")]
        [Column("image")]
        public string? Image { get; set; }

        [Required(ErrorMessage = "Il prezzo è obbligatorio")]
        [Column("price")]
        [Precision(9, 2)]
        public decimal Price { get; set; }

        public int? CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        public List<Ingredient>? Ingredients { get; set; }
    }
}

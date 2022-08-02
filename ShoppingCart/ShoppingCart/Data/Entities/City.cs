using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Data.Entities
{
    public class City
    {
        public int Id { get; set; }

        [Display(Name = "Ciuadad")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }

        public State State { get; set; }
    }
}

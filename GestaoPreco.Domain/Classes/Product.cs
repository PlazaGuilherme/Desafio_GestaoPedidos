using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Product : Entity
    {
        [Required(ErrorMessage = "O nome do produto � obrigat�rio.")]
        [StringLength(100, ErrorMessage = "O nome do produto deve ter no m�ximo 100 caracteres.")]
        public string Name { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "O pre�o deve ser maior que zero.")]
        public decimal Price { get; set; }
    }
}
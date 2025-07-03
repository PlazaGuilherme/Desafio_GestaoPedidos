using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class OrderItem : Entity
    {
        [Required(ErrorMessage = "O ID do pedido � obrigat�rio.")]
        public Guid OrderId { get; set; }

        [Required(ErrorMessage = "O ID do produto � obrigat�rio.")]
        public Guid ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser pelo menos 1.")]
        public int Quantity { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "O pre�o unit�rio deve ser maior que zero.")]
        public decimal UnitPrice { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
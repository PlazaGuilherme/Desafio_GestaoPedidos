using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class OrderItem : Entity
    {
        [Required(ErrorMessage = "O ID do pedido é obrigatório.")]
        public Guid OrderId { get; set; }

        [Required(ErrorMessage = "O ID do produto é obrigatório.")]
        public Guid ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser pelo menos 1.")]
        public int Quantity { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "O preço unitário deve ser maior que zero.")]
        public decimal UnitPrice { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
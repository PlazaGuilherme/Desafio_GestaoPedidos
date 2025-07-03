using Domain;
using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class CreateOrderWithItemsDto
    {
        [Required]
        public Guid CustomerId { get; set; }
        
        [Required]
        public DateTime Date { get; set; }
        
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "TotalAmount deve ser maior que zero")]
        public decimal TotalAmount { get; set; }
        
        [Required]
        [MinLength(1, ErrorMessage = "Pelo menos um item deve ser adicionado")]
        public List<CreateOrderItemDto> Items { get; set; } = new();
        
        [Required]
        public OrderStatus Status { get; set; }
    }
}
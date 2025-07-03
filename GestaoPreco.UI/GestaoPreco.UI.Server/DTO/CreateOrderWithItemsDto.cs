using Domain;

namespace DTO
{
    public class CreateOrderWithItemsDto
    {
        public Guid CustomerId { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalAmount { get; set; }
        public List<CreateOrderItemDto> Items { get; set; } = new();
        public OrderStatus Status { get; set; }
    }

}
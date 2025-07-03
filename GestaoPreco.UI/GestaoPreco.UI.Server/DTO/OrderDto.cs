namespace DTO
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalAmount { get; set; }

        public static OrderDto FromEntity(Domain.Order o) => new()
        {
            Id = o.Id,
            CustomerId = o.CustomerId,
            Date = o.OrderDate,
            TotalAmount = o.TotalAmount
        };
    }

    public class CreateOrderDto
    {
        public Guid CustomerId { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class UpdateOrderDto
    {
        public Guid CustomerId { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
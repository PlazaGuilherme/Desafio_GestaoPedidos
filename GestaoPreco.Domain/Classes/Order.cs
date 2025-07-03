namespace Domain
{
    public enum OrderStatus
    {
        Pendente,
        Processando,
        Concluido,
        Cancelado
    }

    public class Order : Entity
    {
        public Guid CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
        public IEnumerable<string> Validate()
        {
            if (CustomerId == Guid.Empty)
                yield return "CustomerId não pode ser vazio.";

            if (OrderDate > DateTime.UtcNow)
                yield return "OrderDate não pode ser no futuro.";

            if (TotalAmount <= 0)
                yield return "TotalAmount deve ser maior que zero.";

            if (!Enum.IsDefined(typeof(OrderStatus), Status))
                yield return "Status do pedido inválido.";
        }
    }
}
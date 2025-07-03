using MediatR;
using System;

namespace GestaoPreco.Application.Commands.OrderItem
{
    public class UpdateOrderItemCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

        public UpdateOrderItemCommand() { }

        public UpdateOrderItemCommand(Guid id, Guid orderId, Guid productId, string productName, int quantity, decimal unitPrice, decimal totalPrice)
        {
            Id = id;
            OrderId = orderId;
            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            UnitPrice = unitPrice;
            TotalPrice = totalPrice;
        }
    }
}
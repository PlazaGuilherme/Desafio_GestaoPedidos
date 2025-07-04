using MediatR;
using System;

namespace GestaoPreco.Application.Commands.OrderItemCommand
{
    public class CreateOrderItemCommand : IRequest<Guid>
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

        public CreateOrderItemCommand(Guid orderId, Guid productId, string productName, int quantity, decimal unitPrice, decimal totalPrice)
        {
            OrderId = orderId;
            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            UnitPrice = unitPrice;
            TotalPrice = totalPrice;
        }
    }
}
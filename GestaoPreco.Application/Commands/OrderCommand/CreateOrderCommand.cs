using Domain;
using MediatR;
using System;

namespace GestaoPreco.Application.Commands.Order
{
    public class CreateOrderCommand : IRequest<Guid>
    {
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public Guid? CustomerId { get; set; }

        public CreateOrderCommand(DateTime orderDate, decimal totalAmount, OrderStatus status, Guid? customerId)
        {
            OrderDate = orderDate;
            TotalAmount = totalAmount;
            Status = status;
            CustomerId = customerId;
        }
    }
}
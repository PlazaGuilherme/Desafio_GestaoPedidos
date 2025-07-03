using Domain;
using MediatR;
using System;

namespace GestaoPreco.Application.Commands.Order
{
    public class UpdateOrderCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public Guid? CustomerId { get; set; }

        public UpdateOrderCommand(Guid id, DateTime orderDate, decimal totalAmount, OrderStatus status, Guid? customerId)
        {
            Id = id;
            OrderDate = orderDate;
            TotalAmount = totalAmount;
            Status = status;
            CustomerId = customerId;
        }
    }
}
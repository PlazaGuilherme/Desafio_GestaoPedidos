using MediatR;
using System;

namespace GestaoPreco.Application.Commands.OrderItem
{
    public class DeleteOrderItemCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public DeleteOrderItemCommand(Guid id)
        {
            Id = id;
        }
    }
}
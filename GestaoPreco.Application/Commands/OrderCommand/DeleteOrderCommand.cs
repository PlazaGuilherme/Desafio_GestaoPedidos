using MediatR;
using System;

namespace GestaoPreco.Application.Commands.Order
{
    public class DeleteOrderCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
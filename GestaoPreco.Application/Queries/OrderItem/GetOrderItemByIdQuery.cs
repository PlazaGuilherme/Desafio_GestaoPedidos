using MediatR;
using Domain;
using System;

namespace GestaoPreco.Application.Queries.OrderItem
{
    public class GetOrderItemByIdQuery : IRequest<Domain.OrderItem>
    {
        public Guid Id { get; set; }
    }
}
using Domain;
using MediatR;
using System;

namespace Application
{
    public class GetOrderByIdQuery : IRequest<Order?>
    {
        public Guid Id { get; set; }
    }
}
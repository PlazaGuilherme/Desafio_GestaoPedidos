using MediatR;
using Domain;
using System.Collections.Generic;

namespace GestaoPreco.Application.Queries.OrderItem
{
    public class ListOrderItemsQuery : IRequest<IEnumerable<Domain.OrderItem>>
    {
    }
}
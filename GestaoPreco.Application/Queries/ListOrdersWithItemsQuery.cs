using Domain;
using MediatR;
using System.Collections.Generic;

namespace Application.Queries
{
    public class ListOrdersWithItemsQuery : IRequest<IEnumerable<Order>>
    {
    }
}
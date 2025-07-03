using Domain;
using MediatR;
using System.Collections.Generic;

namespace Application
{
    public class ListOrdersQuery : IRequest<IEnumerable<Order>>
    {
    }
}
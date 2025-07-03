using Domain;
using MediatR;
using System.Collections.Generic;

namespace GestaoPreco.Application.Queries.Customer
{
    public class ListCustomersQuery : IRequest<IEnumerable<Domain.Customer>> { }
}
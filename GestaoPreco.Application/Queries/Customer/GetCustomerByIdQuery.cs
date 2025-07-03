using Domain;
using MediatR;
using System;

namespace GestaoPreco.Application.Queries.Customer
{
    public class GetCustomerByIdQuery : IRequest<Domain.Customer?>
    {
        public Guid Id { get; set; }
    }
}
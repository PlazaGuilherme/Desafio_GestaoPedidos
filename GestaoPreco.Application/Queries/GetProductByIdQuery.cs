using Domain;
using MediatR;
using System;

namespace Application.Queries.Product
{
    public class GetProductByIdQuery : IRequest<Domain.Product?>
    {
        public Guid Id { get; set; }
    }
}
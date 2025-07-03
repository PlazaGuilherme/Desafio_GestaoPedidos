using Domain;
using MediatR;


namespace Application.Queries.Product
{
    public class ListProductsQuery : IRequest<IEnumerable<Domain.Product>>
    {
    }
}
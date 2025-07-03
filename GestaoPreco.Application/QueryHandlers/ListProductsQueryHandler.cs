using Application.Queries.Product;
using Domain;
using Infrastructure;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.QueryHandlers.Product
{
    public class ListProductsQueryHandler : IRequestHandler<ListProductsQuery, IEnumerable<Domain.Product>>
    {
        private readonly IProductRepository _repository;

        public ListProductsQueryHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Domain.Product>> Handle(ListProductsQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync();
        }
    }
}
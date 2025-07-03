using Domain;
using Infrastructure;

namespace Application.QueryHandlers
{
    public class ListProductsQueryHandler
    {
        private readonly IProductRepository _repository;

        public ListProductsQueryHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Product>> Handle()
        {
            return await _repository.GetAllAsync();
        }
    }
}
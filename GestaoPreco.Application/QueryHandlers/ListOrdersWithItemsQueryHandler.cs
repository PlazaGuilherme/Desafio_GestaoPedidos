using Application.Queries;
using Domain;
using Infrastructure;
using MediatR;

namespace Application.QueryHandlers
{
    public class ListOrdersWithItemsQueryHandler : IRequestHandler<ListOrdersWithItemsQuery, IEnumerable<Order>>
    {
        private readonly IOrderRepository _repository;

        public ListOrdersWithItemsQueryHandler(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Order>> Handle(ListOrdersWithItemsQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllWithItemsAsync();
        }
    }
}
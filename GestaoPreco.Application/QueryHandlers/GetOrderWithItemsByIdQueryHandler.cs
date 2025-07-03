using Application.Queries;
using Domain;
using Infrastructure;
using MediatR;

namespace Application.QueryHandlers
{
    public class GetOrderWithItemsByIdQueryHandler : IRequestHandler<GetOrderWithItemsByIdQuery, Order?>
    {
        private readonly IOrderRepository _repository;

        public GetOrderWithItemsByIdQueryHandler(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<Order?> Handle(GetOrderWithItemsByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetByIdWithItemsAsync(request.OrderId);
        }
    }
}
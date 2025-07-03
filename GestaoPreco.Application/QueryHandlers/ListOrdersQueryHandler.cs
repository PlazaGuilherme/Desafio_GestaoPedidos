using Application;
using Domain;
using Infrastructure;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.QueryHandlers
{
    public class ListOrdersQueryHandler : IRequestHandler<ListOrdersQuery, IEnumerable<Order>>
    {
        private readonly IOrderRepository _repository;

        public ListOrdersQueryHandler(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Order>> Handle(ListOrdersQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync();
        }
    }
}
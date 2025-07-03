using MediatR;
using Domain;
using GestaoPreco.Application.Queries.OrderItem;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure;

namespace GestaoPreco.Application.QueryHandlers.OrderItem
{
    public class ListOrderItemsQueryHandler : IRequestHandler<ListOrderItemsQuery, IEnumerable<Domain.OrderItem>>
    {
        private readonly IOrderItemRepository _orderItemRepository;

        public ListOrderItemsQueryHandler(IOrderItemRepository orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }

        public async Task<IEnumerable<Domain.OrderItem>> Handle(ListOrderItemsQuery request, CancellationToken cancellationToken)
        {
            return await _orderItemRepository.GetAllAsync();
        }
    }
}
using Domain;
using GestaoPreco.Application.Commands.OrderItemCommand;
using Infrastructure;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GestaoPreco.Application.CommandHandlers.OrderItemHandlers
{
    public class CreateOrderItemCommandHandler : IRequestHandler<CreateOrderItemCommand, Guid>
    {
        private readonly IOrderItemRepository _repository;

        public CreateOrderItemCommandHandler(IOrderItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(CreateOrderItemCommand command, CancellationToken cancellationToken)
        {
            var orderItem = new OrderItem
            {
                OrderId = command.OrderId,
                ProductId = command.ProductId,
                ProductName = command.ProductName,
                Quantity = command.Quantity,
                UnitPrice = command.UnitPrice,
                TotalPrice = command.TotalPrice
            };

            // Adicione validação se necessário

            await _repository.AddAsync(orderItem);
            return orderItem.Id;
        }
    }
}
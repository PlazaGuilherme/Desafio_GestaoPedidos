using Domain;
using GestaoPreco.Application.Commands.Order;
using Infrastructure;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace GestaoPreco.Application.CommandHandlers.OrderHandler
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
    {
        private readonly IOrderRepository _repository;

        public CreateOrderCommandHandler(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            var order = new Order
            {
                CustomerId = command.CustomerId ?? Guid.Empty,
                OrderDate = command.OrderDate,
                TotalAmount = command.TotalAmount,
                Status = command.Status
            };

            var validationErrors = order.Validate();
            if (validationErrors is not null)
            {
                foreach (var error in validationErrors)
                    if (!string.IsNullOrWhiteSpace(error))
                        throw new ValidationException(error);
            }

            await _repository.AddAsync(order);
            return order.Id;
        }
    }
}
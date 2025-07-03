using Domain;
using GestaoPreco.Application.Commands.Order;
using Infrastructure;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace GestaoPreco.Application.CommandHandlers.OrderHandler
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, bool>
    {
        private readonly IOrderRepository _repository;

        public UpdateOrderCommandHandler(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
        {
            var order = await _repository.GetByIdAsync(command.Id);
            if (order == null)
                return false;

            order.CustomerId = command.CustomerId ?? order.CustomerId;
            order.OrderDate = command.OrderDate;
            order.TotalAmount = command.TotalAmount;
            order.Status = command.Status;

            var validationErrors = order.Validate();
            if (validationErrors is not null)
            {
                foreach (var error in validationErrors)
                    if (!string.IsNullOrWhiteSpace(error))
                        throw new ValidationException(error);
            }

            await _repository.UpdateAsync(order);
            return true;
        }
    }
}
using GestaoPreco.Application.Commands.Order;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GestaoPreco.Application.CommandHandlers.OrderHandler
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, bool>
    {
        private readonly IOrderRepository _repository;

        public DeleteOrderCommandHandler(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
        {
            var order = await _repository.GetByIdAsync(command.Id);
            if (order == null)
                return false;

            await _repository.DeleteAsync(command.Id);
            return true;
        }
    }
}
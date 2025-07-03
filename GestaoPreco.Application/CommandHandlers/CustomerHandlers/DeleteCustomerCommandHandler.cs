using GestaoPreco.Application.Commands.Customer;
using Infrastructure;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GestaoPreco.Application.CommandHandlers.CustomerHandlers
{
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, bool>
    {
        private readonly ICustomerRepository _repository;

        public DeleteCustomerCommandHandler(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _repository.GetByIdAsync(request.Id);
            if (customer == null)
                return false;

            await _repository.DeleteAsync(request.Id);
            return true;
        }
    }
}
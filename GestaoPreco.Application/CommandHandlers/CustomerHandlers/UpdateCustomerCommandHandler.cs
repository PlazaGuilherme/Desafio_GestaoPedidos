using GestaoPreco.Application.Commands.Customer;
using Infrastructure;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GestaoPreco.Application.CommandHandlers.CustomerHandlers
{
    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, bool>
    {
        private readonly ICustomerRepository _repository;

        public UpdateCustomerCommandHandler(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _repository.GetByIdAsync(request.Id);
            if (customer == null)
                return false;

            customer.Name = request.Name;
            customer.Email = request.Email;
            customer.Phone = request.Phone;

            await _repository.UpdateAsync(customer);
            return true;
        }
    }
}
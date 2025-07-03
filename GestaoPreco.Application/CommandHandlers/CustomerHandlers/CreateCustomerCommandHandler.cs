using Domain;
using GestaoPreco.Application.Commands.CustomerCommand;
using Infrastructure;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GestaoPreco.Application.CommandHandlers.CustomerHandlers
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Guid>
    {
        private readonly ICustomerRepository _repository;

        public CreateCustomerCommandHandler(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
        {
            var customer = new Customer
            {
                Name = command.Name,
                Email = command.Email,
                Phone = command.Phone
            };

            // Adicione validação se necessário

            await _repository.AddAsync(customer);
            return customer.Id;
        }
    }
}
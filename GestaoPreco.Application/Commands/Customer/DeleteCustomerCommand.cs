using MediatR;
using System;

namespace GestaoPreco.Application.Commands.Customer
{
    public class DeleteCustomerCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public DeleteCustomerCommand(Guid id) => Id = id;
    }
}
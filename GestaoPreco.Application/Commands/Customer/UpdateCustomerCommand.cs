using MediatR;
using System;

namespace GestaoPreco.Application.Commands.Customer
{
    public class UpdateCustomerCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }
}
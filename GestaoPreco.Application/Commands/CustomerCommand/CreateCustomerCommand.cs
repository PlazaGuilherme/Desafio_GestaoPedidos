using MediatR;
using System;

namespace GestaoPreco.Application.Commands.CustomerCommand
{
    public class CreateCustomerCommand : IRequest<Guid>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public CreateCustomerCommand(string name, string email, string phone)
        {
            Name = name;
            Email = email;
            Phone = phone;
        }
    }
}
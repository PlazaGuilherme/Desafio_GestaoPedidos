using Domain;
using FluentAssertions;
using GestaoPreco.Application.CommandHandlers.CustomerHandlers;
using GestaoPreco.Application.Commands.Customer;
using Infrastructure;
using Moq;

namespace GestaoPreco.UnitTests.Application.Customers;

public class UpdateCustomerCommandHandlerTests
{
    [Fact]
    public async Task Handle_Deve_Retornar_False_Quando_Cliente_Nao_Existir()
    {
        var id = Guid.NewGuid();
        var repository = new Mock<ICustomerRepository>();
        repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Customer?)null);

        var handler = new UpdateCustomerCommandHandler(repository.Object);
        var command = new UpdateCustomerCommand
        {
            Id = id,
            Name = "X",
            Email = "x@x.com",
            Phone = "11"
        };

        var result = await handler.Handle(command, default);

        result.Should().BeFalse();
        repository.Verify(r => r.UpdateAsync(It.IsAny<Customer>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Deve_Atualizar_E_Retornar_True_Quando_Existir()
    {
        var id = Guid.NewGuid();
        var customer = new Customer
        {
            Id = id,
            Name = "Velho",
            Email = "old@email.com",
            Phone = "11000000000"
        };

        var repository = new Mock<ICustomerRepository>();
        repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(customer);
        repository.Setup(r => r.UpdateAsync(It.IsAny<Customer>())).Returns(Task.CompletedTask);

        var handler = new UpdateCustomerCommandHandler(repository.Object);
        var command = new UpdateCustomerCommand
        {
            Id = id,
            Name = "Novo",
            Email = "new@email.com",
            Phone = "11999999999"
        };

        var result = await handler.Handle(command, default);

        result.Should().BeTrue();
        customer.Name.Should().Be("Novo");
        customer.Email.Should().Be("new@email.com");
        customer.Phone.Should().Be("11999999999");
        repository.Verify(r => r.UpdateAsync(customer), Times.Once);
    }
}

using Domain;
using FluentAssertions;
using GestaoPreco.Application.CommandHandlers.CustomerHandlers;
using GestaoPreco.Application.Commands.Customer;
using Infrastructure;
using Moq;

namespace GestaoPreco.UnitTests.Application.Customers;

public class DeleteCustomerCommandHandlerTests
{
    [Fact]
    public async Task Handle_Deve_Retornar_False_Quando_Cliente_Nao_Existir()
    {
        var id = Guid.NewGuid();
        var repository = new Mock<ICustomerRepository>();
        repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Customer?)null);

        var handler = new DeleteCustomerCommandHandler(repository.Object);

        var result = await handler.Handle(new DeleteCustomerCommand(id), default);

        result.Should().BeFalse();
        repository.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Deve_Excluir_E_Retornar_True_Quando_Existir()
    {
        var id = Guid.NewGuid();
        var customer = new Customer
        {
            Id = id,
            Name = "Del",
            Email = "d@e.com",
            Phone = "11"
        };

        var repository = new Mock<ICustomerRepository>();
        repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(customer);
        repository.Setup(r => r.DeleteAsync(id)).Returns(Task.CompletedTask);

        var handler = new DeleteCustomerCommandHandler(repository.Object);

        var result = await handler.Handle(new DeleteCustomerCommand(id), default);

        result.Should().BeTrue();
        repository.Verify(r => r.DeleteAsync(id), Times.Once);
    }
}

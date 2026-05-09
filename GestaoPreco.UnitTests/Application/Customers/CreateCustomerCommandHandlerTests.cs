using Domain;
using FluentAssertions;
using GestaoPreco.Application.CommandHandlers.CustomerHandlers;
using GestaoPreco.Application.Commands.Customer;
using Infrastructure;
using Moq;

namespace GestaoPreco.UnitTests.Application.Customers;

public class CreateCustomerCommandHandlerTests
{
    [Fact]
    public async Task Handle_Deve_Persistir_E_Retornar_Id()
    {
        var command = new CreateCustomerCommand
        {
            Name = "Carla",
            Email = "carla@email.com",
            Phone = "11777777777"
        };

        var repository = new Mock<ICustomerRepository>();
        repository.Setup(r => r.AddAsync(It.IsAny<Customer>())).Returns(Task.CompletedTask);

        var handler = new CreateCustomerCommandHandler(repository.Object);

        var id = await handler.Handle(command, default);

        id.Should().NotBeEmpty();
        repository.Verify(
            r => r.AddAsync(It.Is<Customer>(c =>
                c.Name == "Carla"
                && c.Email == "carla@email.com"
                && c.Phone == "11777777777")),
            Times.Once);
    }
}

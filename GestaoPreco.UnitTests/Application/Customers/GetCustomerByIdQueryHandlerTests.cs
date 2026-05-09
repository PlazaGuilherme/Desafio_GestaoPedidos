using Domain;
using FluentAssertions;
using GestaoPreco.Application.Queries.Customer;
using GestaoPreco.Application.QueryHandlers.Customer;
using Infrastructure;
using Moq;

namespace GestaoPreco.UnitTests.Application.Customers;

public class GetCustomerByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_Deve_Retornar_Null_Quando_Nao_Existir()
    {
        var id = Guid.NewGuid();
        var repository = new Mock<ICustomerRepository>();
        repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Customer?)null);

        var handler = new GetCustomerByIdQueryHandler(repository.Object);

        var result = await handler.Handle(new GetCustomerByIdQuery { Id = id }, default);

        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_Deve_Retornar_Cliente_Quando_Existir()
    {
        var id = Guid.NewGuid();
        var customer = new Customer
        {
            Id = id,
            Name = "Bruno",
            Email = "bruno@email.com",
            Phone = "11888888888"
        };

        var repository = new Mock<ICustomerRepository>();
        repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(customer);

        var handler = new GetCustomerByIdQueryHandler(repository.Object);

        var result = await handler.Handle(new GetCustomerByIdQuery { Id = id }, default);

        result.Should().BeEquivalentTo(customer);
    }
}

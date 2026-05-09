using Domain;
using FluentAssertions;
using GestaoPreco.Application.Queries.Customer;
using Infrastructure;
using Moq;

namespace GestaoPreco.UnitTests.Application.Customers;

public class ListCustomersQueryHandlerTests
{
    [Fact]
    public async Task Handle_Deve_Retornar_Clientes_Do_Repositorio()
    {
        var customers = new List<Customer>
        {
            new()
            {
                Name = "Ana",
                Email = "ana@email.com",
                Phone = "11999999999"
            }
        };

        var repository = new Mock<ICustomerRepository>();
        repository.Setup(r => r.GetAllAsync()).ReturnsAsync(customers);

        var handler = new ListCustomersQueryHandler(repository.Object);

        var result = await handler.Handle(new ListCustomersQuery(), default);

        result.Should().BeEquivalentTo(customers);
        repository.Verify(r => r.GetAllAsync(), Times.Once);
    }
}

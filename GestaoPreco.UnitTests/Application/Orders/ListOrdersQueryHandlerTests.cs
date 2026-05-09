using Application;
using Application.QueryHandlers;
using Domain;
using FluentAssertions;
using Infrastructure;
using Moq;

namespace GestaoPreco.UnitTests.Application.Orders;

public class ListOrdersQueryHandlerTests
{
    [Fact]
    public async Task Handle_Deve_Retornar_Pedidos_Do_Repositorio()
    {
        var orders = new List<Order>
        {
            new()
            {
                CustomerId = Guid.NewGuid(),
                OrderDate = DateTime.UtcNow,
                TotalAmount = 10m,
                Status = OrderStatus.Pendente
            }
        };

        var repository = new Mock<IOrderRepository>();
        repository.Setup(r => r.GetAllAsync()).ReturnsAsync(orders);

        var handler = new ListOrdersQueryHandler(repository.Object);

        var result = await handler.Handle(new ListOrdersQuery(), default);

        result.Should().BeEquivalentTo(orders);
        repository.Verify(r => r.GetAllAsync(), Times.Once);
    }
}

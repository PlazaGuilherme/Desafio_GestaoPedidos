using Application.Queries;
using Application.QueryHandlers;
using Domain;
using FluentAssertions;
using Infrastructure;
using Moq;

namespace GestaoPreco.UnitTests.Application.Orders;

public class ListOrdersWithItemsQueryHandlerTests
{
    [Fact]
    public async Task Handle_Deve_Retornar_Pedidos_Com_Itens_Do_Repositorio()
    {
        var orders = new List<Order>
        {
            new()
            {
                CustomerId = Guid.NewGuid(),
                OrderDate = DateTime.UtcNow,
                TotalAmount = 20m,
                Status = OrderStatus.Pendente,
                Items = new List<OrderItem>()
            }
        };

        var repository = new Mock<IOrderRepository>();
        repository.Setup(r => r.GetAllWithItemsAsync()).ReturnsAsync(orders);

        var handler = new ListOrdersWithItemsQueryHandler(repository.Object);

        var result = await handler.Handle(new ListOrdersWithItemsQuery(), default);

        result.Should().BeEquivalentTo(orders);
        repository.Verify(r => r.GetAllWithItemsAsync(), Times.Once);
    }
}

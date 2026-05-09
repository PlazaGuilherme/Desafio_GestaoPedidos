using Domain;
using FluentAssertions;
using GestaoPreco.Application.Queries.OrderItem;
using GestaoPreco.Application.QueryHandlers.OrderItem;
using Infrastructure;
using Moq;

namespace GestaoPreco.UnitTests.Application.OrderItems;

public class ListOrderItemsQueryHandlerTests
{
    [Fact]
    public async Task Handle_Deve_Retornar_Itens_Do_Repositorio()
    {
        var items = new List<OrderItem>
        {
            new()
            {
                OrderId = Guid.NewGuid(),
                ProductId = Guid.NewGuid(),
                ProductName = "Produto",
                Quantity = 2,
                UnitPrice = 10m,
                TotalPrice = 20m
            }
        };

        var repository = new Mock<IOrderItemRepository>();
        repository.Setup(r => r.GetAllAsync()).ReturnsAsync(items);

        var handler = new ListOrderItemsQueryHandler(repository.Object);

        var result = await handler.Handle(new ListOrderItemsQuery(), default);

        result.Should().BeEquivalentTo(items);
        repository.Verify(r => r.GetAllAsync(), Times.Once);
    }
}

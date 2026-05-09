using Application.Queries;
using Application.QueryHandlers;
using Domain;
using FluentAssertions;
using Infrastructure;
using Moq;

namespace GestaoPreco.UnitTests.Application.Orders;

public class GetOrderWithItemsByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_Deve_Retornar_Null_Quando_Nao_Existir()
    {
        var orderId = Guid.NewGuid();
        var repository = new Mock<IOrderRepository>();
        repository.Setup(r => r.GetByIdWithItemsAsync(orderId)).ReturnsAsync((Order?)null);

        var handler = new GetOrderWithItemsByIdQueryHandler(repository.Object);

        var result = await handler.Handle(new GetOrderWithItemsByIdQuery(orderId), default);

        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_Deve_Retornar_Pedido_Quando_Existir()
    {
        var orderId = Guid.NewGuid();
        var order = new Order
        {
            Id = orderId,
            CustomerId = Guid.NewGuid(),
            OrderDate = DateTime.UtcNow,
            TotalAmount = 30m,
            Status = OrderStatus.Concluido,
            Items = new List<OrderItem>()
        };

        var repository = new Mock<IOrderRepository>();
        repository.Setup(r => r.GetByIdWithItemsAsync(orderId)).ReturnsAsync(order);

        var handler = new GetOrderWithItemsByIdQueryHandler(repository.Object);

        var result = await handler.Handle(new GetOrderWithItemsByIdQuery(orderId), default);

        result.Should().BeEquivalentTo(order);
    }
}

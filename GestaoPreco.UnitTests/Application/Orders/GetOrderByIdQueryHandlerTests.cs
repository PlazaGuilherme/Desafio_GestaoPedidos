using Application;
using Domain;
using FluentAssertions;
using GestaoPreco.Application.QueryHandlers;
using Infrastructure;
using Moq;

namespace GestaoPreco.UnitTests.Application.Orders;

public class GetOrderByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_Deve_Retornar_Null_Quando_Nao_Existir()
    {
        var id = Guid.NewGuid();
        var repository = new Mock<IOrderRepository>();
        repository.Setup(r => r.GetByIdWithItemsAsync(id)).ReturnsAsync((Order?)null);

        var handler = new GetOrderByIdQueryHandler(repository.Object);

        var result = await handler.Handle(new GetOrderByIdQuery { Id = id }, default);

        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_Deve_Retornar_Pedido_Quando_Existir()
    {
        var id = Guid.NewGuid();
        var order = new Order
        {
            Id = id,
            CustomerId = Guid.NewGuid(),
            OrderDate = DateTime.UtcNow,
            TotalAmount = 50m,
            Status = OrderStatus.Pendente
        };

        var repository = new Mock<IOrderRepository>();
        repository.Setup(r => r.GetByIdWithItemsAsync(id)).ReturnsAsync(order);

        var handler = new GetOrderByIdQueryHandler(repository.Object);

        var result = await handler.Handle(new GetOrderByIdQuery { Id = id }, default);

        result.Should().BeEquivalentTo(order);
    }
}

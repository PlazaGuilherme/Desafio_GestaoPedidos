using FluentAssertions;
using GestaoPreco.Application.CommandHandlers.OrderHandler;
using GestaoPreco.Application.Commands.Order;
using Domain;
using Infrastructure;
using Moq;

namespace GestaoPreco.UnitTests.Application.Orders;

public class DeleteOrderCommandHandlerTests
{
    [Fact]
    public async Task Handle_Deve_Retornar_False_Quando_Pedido_Nao_Existir()
    {
        var id = Guid.NewGuid();
        var repository = new Mock<IOrderRepository>();
        repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Order?)null);

        var handler = new DeleteOrderCommandHandler(repository.Object);

        var result = await handler.Handle(new DeleteOrderCommand { Id = id }, default);

        result.Should().BeFalse();
        repository.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Deve_Excluir_E_Retornar_True_Quando_Existir()
    {
        var id = Guid.NewGuid();
        var order = new Order
        {
            Id = id,
            CustomerId = Guid.NewGuid(),
            OrderDate = DateTime.UtcNow,
            TotalAmount = 5m,
            Status = OrderStatus.Pendente
        };

        var repository = new Mock<IOrderRepository>();
        repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(order);
        repository.Setup(r => r.DeleteAsync(id)).Returns(Task.CompletedTask);

        var handler = new DeleteOrderCommandHandler(repository.Object);

        var result = await handler.Handle(new DeleteOrderCommand { Id = id }, default);

        result.Should().BeTrue();
        repository.Verify(r => r.DeleteAsync(id), Times.Once);
    }
}

using Domain;
using FluentAssertions;
using GestaoPreco.Application.CommandHandlers.OrderHandler;
using GestaoPreco.Application.Commands.Order;
using Infrastructure;
using Moq;
using System.ComponentModel.DataAnnotations;

namespace GestaoPreco.UnitTests.Application.Orders;

public class UpdateOrderCommandHandlerTests
{
    [Fact]
    public async Task Handle_Deve_Retornar_False_Quando_Pedido_Nao_Existir()
    {
        var id = Guid.NewGuid();
        var repository = new Mock<IOrderRepository>();
        repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Order?)null);

        var handler = new UpdateOrderCommandHandler(repository.Object);
        var command = new UpdateOrderCommand(
            id,
            DateTime.UtcNow,
            15m,
            OrderStatus.Processando,
            Guid.NewGuid());

        var result = await handler.Handle(command, default);

        result.Should().BeFalse();
        repository.Verify(r => r.UpdateAsync(It.IsAny<Order>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Deve_Atualizar_E_Retornar_True_Quando_Existir()
    {
        var id = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var existing = new Order
        {
            Id = id,
            CustomerId = customerId,
            OrderDate = DateTime.UtcNow.AddDays(-2),
            TotalAmount = 10m,
            Status = OrderStatus.Pendente
        };

        var repository = new Mock<IOrderRepository>();
        repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existing);
        repository.Setup(r => r.UpdateAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);

        var handler = new UpdateOrderCommandHandler(repository.Object);
        var command = new UpdateOrderCommand(
            id,
            DateTime.UtcNow,
            25m,
            OrderStatus.Concluido,
            customerId);

        var result = await handler.Handle(command, default);

        result.Should().BeTrue();
        existing.TotalAmount.Should().Be(25m);
        existing.Status.Should().Be(OrderStatus.Concluido);
        repository.Verify(r => r.UpdateAsync(existing), Times.Once);
    }

    [Fact]
    public async Task Handle_Deve_Lancar_Quando_Dados_Invalidos_Apos_Atualizar()
    {
        var id = Guid.NewGuid();
        var existing = new Order
        {
            Id = id,
            CustomerId = Guid.NewGuid(),
            OrderDate = DateTime.UtcNow,
            TotalAmount = 10m,
            Status = OrderStatus.Pendente
        };

        var repository = new Mock<IOrderRepository>();
        repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existing);

        var handler = new UpdateOrderCommandHandler(repository.Object);
        var command = new UpdateOrderCommand(
            id,
            DateTime.UtcNow,
            0m,
            OrderStatus.Pendente,
            existing.CustomerId);

        var act = async () => await handler.Handle(command, default);

        await act.Should().ThrowAsync<ValidationException>();
    }
}

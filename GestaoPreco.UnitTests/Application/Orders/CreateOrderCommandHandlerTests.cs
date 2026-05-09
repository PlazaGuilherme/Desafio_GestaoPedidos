using Domain;
using FluentAssertions;
using GestaoPreco.Application.CommandHandlers.OrderHandler;
using GestaoPreco.Application.Commands.Order;
using Infrastructure;
using Moq;
using System.ComponentModel.DataAnnotations;

namespace GestaoPreco.UnitTests.Application.Orders;

public class CreateOrderCommandHandlerTests
{
    [Fact]
    public async Task Handle_Deve_Persistir_E_Retornar_Id_Quando_Valido()
    {
        var customerId = Guid.NewGuid();
        var command = new CreateOrderCommand(
            DateTime.UtcNow,
            99.90m,
            OrderStatus.Pendente,
            customerId);

        var repository = new Mock<IOrderRepository>();
        repository.Setup(r => r.AddAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);

        var handler = new CreateOrderCommandHandler(repository.Object);

        var orderId = await handler.Handle(command, default);

        orderId.Should().NotBeEmpty();
        repository.Verify(
            r => r.AddAsync(It.Is<Order>(o =>
                o.CustomerId == customerId
                && o.TotalAmount == 99.90m
                && o.Status == OrderStatus.Pendente)),
            Times.Once);
    }

    [Fact]
    public async Task Handle_Deve_Lancar_Quando_CustomerId_Vazio()
    {
        var command = new CreateOrderCommand(
            DateTime.UtcNow,
            10m,
            OrderStatus.Pendente,
            Guid.Empty);

        var handler = new CreateOrderCommandHandler(Mock.Of<IOrderRepository>());

        var act = async () => await handler.Handle(command, default);

        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task Handle_Deve_Lancar_Quando_TotalAmount_Invalido()
    {
        var command = new CreateOrderCommand(
            DateTime.UtcNow,
            0m,
            OrderStatus.Pendente,
            Guid.NewGuid());

        var handler = new CreateOrderCommandHandler(Mock.Of<IOrderRepository>());

        var act = async () => await handler.Handle(command, default);

        await act.Should().ThrowAsync<ValidationException>();
    }
}

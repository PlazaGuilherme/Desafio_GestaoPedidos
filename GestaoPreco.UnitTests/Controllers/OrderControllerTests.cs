using Application;
using Application.Queries;
using Domain;
using DTO;
using FluentAssertions;
using GestaoPreco.Application.Commands.Order;
using GestaoPreco.UI.Server.Controllers;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;

namespace GestaoPreco.UnitTests.Controllers.Orders
{
    public class OrderControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly Mock<IOrderItemRepository> _orderItemRepositoryMock;

        private readonly OrderController _controller;

        public OrderControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _configurationMock = new Mock<IConfiguration>();
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _orderItemRepositoryMock = new Mock<IOrderItemRepository>();

            _configurationMock
                .Setup(x => x["OrderSettings:FixedCustomerId"])
                .Returns(Guid.NewGuid().ToString());

            _controller = new OrderController(
                _mediatorMock.Object,
                _configurationMock.Object,
                _orderRepositoryMock.Object,
                _orderItemRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAll_Should_Return_Ok_With_Orders()
        {
            // Arrange
            var orders = new List<Order>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    CustomerId = Guid.NewGuid(),
                    TotalAmount = 100
                }
            };

            _mediatorMock
                .Setup(x => x.Send(
                    It.IsAny<ListOrdersQuery>(),
                    default))
                .ReturnsAsync(orders);

            // Act
            var result = await _controller.GetAll();

            // Assert
            result.Should().BeOfType<OkObjectResult>();

            var okResult = result as OkObjectResult;

            okResult!.Value.Should().BeEquivalentTo(orders);
        }

        [Fact]
        public async Task GetById_Should_Return_NotFound_When_Order_Does_Not_Exist()
        {
            // Arrange
            _mediatorMock
                .Setup(x => x.Send(
                    It.IsAny<GetOrderByIdQuery>(),
                    default))
                .ReturnsAsync((Order?)null);

            // Act
            var result = await _controller.GetById(Guid.NewGuid());

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetById_Should_Return_Ok_When_Order_Exists()
        {
            // Arrange
            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                TotalAmount = 200
            };

            _mediatorMock
                .Setup(x => x.Send(
                    It.IsAny<GetOrderByIdQuery>(),
                    default))
                .ReturnsAsync(order);

            // Act
            var result = await _controller.GetById(order.Id);

            // Assert
            result.Should().BeOfType<OkObjectResult>();

            var okResult = result as OkObjectResult;

            okResult!.Value.Should().BeEquivalentTo(order);
        }

        [Fact]
        public async Task Create_Should_Return_BadRequest_When_ModelState_Is_Invalid()
        {
            // Arrange
            var command = new CreateOrderCommand(DateTime.UtcNow, 0m, default, null);

            _controller.ModelState.AddModelError(
                "CustomerId",
                "Campo obrigatório");

            // Act
            var result = await _controller.Create(command);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Create_Should_Return_Ok_When_Order_Is_Created()
        {
            // Arrange
            var orderId = Guid.NewGuid();

            var command = new CreateOrderCommand(DateTime.UtcNow, 100m, default, null);

            _mediatorMock
                .Setup(x => x.Send(
                    It.IsAny<CreateOrderCommand>(),
                    default))
                .ReturnsAsync(orderId);

            // Act
            var result = await _controller.Create(command);

            // Assert
            result.Should().BeOfType<OkObjectResult>();

            var okResult = result as OkObjectResult;

            okResult!.Value.Should().Be(orderId);
        }

        [Fact]
        public async Task Delete_Should_Return_NotFound_When_Order_Does_Not_Exist()
        {
            // Arrange
            _mediatorMock
                .Setup(x => x.Send(
                    It.IsAny<DeleteOrderCommand>(),
                    default))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(Guid.NewGuid());

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Delete_Should_Return_NoContent_When_Order_Is_Deleted()
        {
            // Arrange
            _mediatorMock
                .Setup(x => x.Send(
                    It.IsAny<DeleteOrderCommand>(),
                    default))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(Guid.NewGuid());

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }
    }
}
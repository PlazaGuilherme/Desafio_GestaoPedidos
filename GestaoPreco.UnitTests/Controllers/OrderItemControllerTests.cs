using Domain;
using FluentAssertions;
using GestaoPreco.Application.Commands.OrderItem;
using GestaoPreco.Application.Queries.OrderItem;
using GestaoPreco.UI.Server.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GestaoPreco.UnitTests.Controllers.OrderItems
{
    public class OrderItemControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly OrderItemController _controller;

        public OrderItemControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();

            _controller = new OrderItemController(
                _mediatorMock.Object);
        }

        [Fact]
        public async Task GetAll_Should_Return_Ok_With_OrderItems()
        {
            // Arrange
            var items = new List<OrderItem>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    OrderId = Guid.NewGuid(),
                    ProductId = Guid.NewGuid(),
                    Quantity = 2,
                    UnitPrice = 50
                }
            };

            _mediatorMock
                .Setup(x => x.Send(
                    It.IsAny<ListOrderItemsQuery>(),
                    default))
                .ReturnsAsync(items);

            // Act
            var result = await _controller.GetAll();

            // Assert
            result.Should().BeOfType<OkObjectResult>();

            var okResult = result as OkObjectResult;

            okResult!.Value.Should().BeEquivalentTo(items);
        }

        [Fact]
        public async Task GetById_Should_Return_NotFound_When_Item_Does_Not_Exist()
        {
            // Arrange
            _mediatorMock
                .Setup(x => x.Send(
                    It.IsAny<GetOrderItemByIdQuery>(),
                    default))
                .ReturnsAsync((OrderItem?)null);

            // Act
            var result = await _controller.GetById(Guid.NewGuid());

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetById_Should_Return_Ok_When_Item_Exists()
        {
            // Arrange
            var item = new OrderItem
            {
                Id = Guid.NewGuid(),
                OrderId = Guid.NewGuid(),
                ProductId = Guid.NewGuid(),
                Quantity = 1,
                UnitPrice = 100
            };

            _mediatorMock
                .Setup(x => x.Send(
                    It.IsAny<GetOrderItemByIdQuery>(),
                    default))
                .ReturnsAsync(item);

            // Act
            var result = await _controller.GetById(item.Id);

            // Assert
            result.Should().BeOfType<OkObjectResult>();

            var okResult = result as OkObjectResult;

            okResult!.Value.Should().BeEquivalentTo(item);
        }

        [Fact]
        public async Task Create_Should_Return_BadRequest_When_ModelState_Is_Invalid()
        {
            // Arrange
            var command = new CreateOrderItemCommand();

            _controller.ModelState.AddModelError(
                "ProductId",
                "Campo obrigatório");

            // Act
            var result = await _controller.Create(command);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Create_Should_Return_BadRequest_When_ItemId_Is_Empty()
        {
            // Arrange
            var command = new CreateOrderItemCommand();

            _mediatorMock
                .Setup(x => x.Send(
                    It.IsAny<CreateOrderItemCommand>(),
                    default))
                .ReturnsAsync(Guid.Empty);

            // Act
            var result = await _controller.Create(command);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task Create_Should_Return_CreatedAtAction_When_Item_Is_Created()
        {
            // Arrange
            var itemId = Guid.NewGuid();

            var item = new OrderItem
            {
                Id = itemId,
                OrderId = Guid.NewGuid(),
                ProductId = Guid.NewGuid(),
                Quantity = 2,
                UnitPrice = 100
            };

            var command = new CreateOrderItemCommand
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            };

            _mediatorMock
                .Setup(x => x.Send(
                    It.IsAny<CreateOrderItemCommand>(),
                    default))
                .ReturnsAsync(itemId);

            _mediatorMock
                .Setup(x => x.Send(
                    It.IsAny<GetOrderItemByIdQuery>(),
                    default))
                .ReturnsAsync(item);

            // Act
            var result = await _controller.Create(command);

            // Assert
            result.Should().BeOfType<CreatedAtActionResult>();

            var createdResult = result as CreatedAtActionResult;

            createdResult!.ActionName.Should()
                .Be(nameof(OrderItemController.GetById));
        }

        [Fact]
        public async Task Update_Should_Return_BadRequest_When_ModelState_Is_Invalid()
        {
            // Arrange
            var command = new UpdateOrderItemCommand();

            _controller.ModelState.AddModelError(
                "Quantity",
                "Campo obrigatório");

            // Act
            var result = await _controller.Update(
                Guid.NewGuid(),
                command);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Update_Should_Return_NotFound_When_Item_Does_Not_Exist()
        {
            // Arrange
            var command = new UpdateOrderItemCommand();

            _mediatorMock
                .Setup(x => x.Send(
                    It.IsAny<UpdateOrderItemCommand>(),
                    default))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Update(
                Guid.NewGuid(),
                command);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Update_Should_Return_NoContent_When_Item_Is_Updated()
        {
            // Arrange
            var command = new UpdateOrderItemCommand();

            _mediatorMock
                .Setup(x => x.Send(
                    It.IsAny<UpdateOrderItemCommand>(),
                    default))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Update(
                Guid.NewGuid(),
                command);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Delete_Should_Return_NotFound_When_Item_Does_Not_Exist()
        {
            // Arrange
            _mediatorMock
                .Setup(x => x.Send(
                    It.IsAny<DeleteOrderItemCommand>(),
                    default))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(Guid.NewGuid());

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Delete_Should_Return_NoContent_When_Item_Is_Deleted()
        {
            // Arrange
            _mediatorMock
                .Setup(x => x.Send(
                    It.IsAny<DeleteOrderItemCommand>(),
                    default))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(Guid.NewGuid());

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }
    }
}
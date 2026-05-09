using Application.Queries.Product;
using Domain;
using FluentAssertions;
using GestaoPreco.Application.Commands.Product;
using GestaoPreco.UI.Server.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GestaoPreco.UnitTests.Controllers.Products
{
    public class ProductControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ProductController _controller;

        public ProductControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();

            _controller = new ProductController(
                _mediatorMock.Object);
        }

        [Fact]
        public async Task GetAll_Should_Return_Ok_With_Products()
        {
            // Arrange
            var products = new List<Product>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Notebook",
                    Price = 5000
                }
            };

            _mediatorMock
                .Setup(x => x.Send(
                    It.IsAny<ListProductsQuery>(),
                    default))
                .ReturnsAsync(products);

            // Act
            var result = await _controller.GetAll();

            // Assert
            result.Should().BeOfType<OkObjectResult>();

            var okResult = result as OkObjectResult;

            okResult!.Value.Should().BeEquivalentTo(products);
        }

        [Fact]
        public async Task GetById_Should_Return_NotFound_When_Product_Does_Not_Exist()
        {
            // Arrange
            _mediatorMock
                .Setup(x => x.Send(
                    It.IsAny<GetProductByIdQuery>(),
                    default))
                .ReturnsAsync((Product?)null);

            // Act
            var result = await _controller.GetById(Guid.NewGuid());

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetById_Should_Return_Ok_When_Product_Exists()
        {
            // Arrange
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Mouse Gamer",
                Price = 150
            };

            _mediatorMock
                .Setup(x => x.Send(
                    It.IsAny<GetProductByIdQuery>(),
                    default))
                .ReturnsAsync(product);

            // Act
            var result = await _controller.GetById(product.Id);

            // Assert
            result.Should().BeOfType<OkObjectResult>();

            var okResult = result as OkObjectResult;

            okResult!.Value.Should().BeEquivalentTo(product);
        }

        [Fact]
        public async Task Create_Should_Return_BadRequest_When_ModelState_Is_Invalid()
        {
            // Arrange
            var command = new CreateProductCommand();

            _controller.ModelState.AddModelError(
                "Name",
                "Campo obrigatório");

            // Act
            var result = await _controller.Create(command);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Create_Should_Return_BadRequest_When_ProductId_Is_Empty()
        {
            // Arrange
            var command = new CreateProductCommand();

            _mediatorMock
                .Setup(x => x.Send(
                    It.IsAny<CreateProductCommand>(),
                    default))
                .ReturnsAsync(Guid.Empty);

            // Act
            var result = await _controller.Create(command);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task Create_Should_Return_CreatedAtAction_When_Product_Is_Created()
        {
            // Arrange
            var productId = Guid.NewGuid();

            var command = new CreateProductCommand
            {
                Name = "Teclado",
                Price = 250
            };

            _mediatorMock
                .Setup(x => x.Send(
                    It.IsAny<CreateProductCommand>(),
                    default))
                .ReturnsAsync(productId);

            // Act
            var result = await _controller.Create(command);

            // Assert
            result.Should().BeOfType<CreatedAtActionResult>();

            var createdResult = result as CreatedAtActionResult;

            createdResult!.ActionName.Should().Be(nameof(ProductController.GetById));
        }

        [Fact]
        public async Task Delete_Should_Return_NotFound_When_Product_Does_Not_Exist()
        {
            // Arrange
            _mediatorMock
                .Setup(x => x.Send(
                    It.IsAny<DeleteProductCommand>(),
                    default))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(Guid.NewGuid());

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Delete_Should_Return_NoContent_When_Product_Is_Deleted()
        {
            // Arrange
            _mediatorMock
                .Setup(x => x.Send(
                    It.IsAny<DeleteProductCommand>(),
                    default))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(Guid.NewGuid());

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }
    }
}
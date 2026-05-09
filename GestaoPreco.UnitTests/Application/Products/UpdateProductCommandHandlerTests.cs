using Domain;
using FluentAssertions;
using GestaoPreco.Application.CommandHandlers.ProductHandlers;
using GestaoPreco.Application.Commands.Product;
using Infrastructure;
using Moq;

namespace GestaoPreco.UnitTests.Application.Products;

public class UpdateProductCommandHandlerTests
{
    [Fact]
    public async Task Handle_Deve_Retornar_False_Quando_Produto_Nao_Existir()
    {
        var id = Guid.NewGuid();
        var repository = new Mock<IProductRepository>();
        repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Product?)null);

        var handler = new UpdateProductCommandHandler(repository.Object);
        var command = new UpdateProductCommand
        {
            Id = id,
            Name = "Novo",
            Price = 1m
        };

        var result = await handler.Handle(command, default);

        result.Should().BeFalse();
        repository.Verify(r => r.UpdateAsync(It.IsAny<Product>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Deve_Atualizar_E_Retornar_True_Quando_Existir()
    {
        var id = Guid.NewGuid();
        var product = new Product
        {
            Id = id,
            Name = "Antigo",
            Price = 10m
        };

        var repository = new Mock<IProductRepository>();
        repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(product);
        repository.Setup(r => r.UpdateAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

        var handler = new UpdateProductCommandHandler(repository.Object);
        var command = new UpdateProductCommand
        {
            Id = id,
            Name = "Novo nome",
            Price = 20m
        };

        var result = await handler.Handle(command, default);

        result.Should().BeTrue();
        product.Name.Should().Be("Novo nome");
        product.Price.Should().Be(20m);
        repository.Verify(r => r.UpdateAsync(product), Times.Once);
    }
}

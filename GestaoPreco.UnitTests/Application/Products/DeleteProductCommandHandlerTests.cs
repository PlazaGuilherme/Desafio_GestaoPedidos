using Domain;
using FluentAssertions;
using GestaoPreco.Application.Commands.Product;
using Infrastructure;
using Moq;

namespace GestaoPreco.UnitTests.Application.Products;

public class DeleteProductCommandHandlerTests
{
    [Fact]
    public async Task Handle_Deve_Retornar_False_Quando_Produto_Nao_Existir()
    {
        var id = Guid.NewGuid();
        var repository = new Mock<IProductRepository>();
        repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Product?)null);

        var handler = new DeleteProductCommandHandler(repository.Object);

        var result = await handler.Handle(new DeleteProductCommand(id), default);

        result.Should().BeFalse();
        repository.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Deve_Excluir_E_Retornar_True_Quando_Existir()
    {
        var id = Guid.NewGuid();
        var product = new Product { Id = id, Name = "X", Price = 1m };

        var repository = new Mock<IProductRepository>();
        repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(product);
        repository.Setup(r => r.DeleteAsync(id)).Returns(Task.CompletedTask);

        var handler = new DeleteProductCommandHandler(repository.Object);

        var result = await handler.Handle(new DeleteProductCommand(id), default);

        result.Should().BeTrue();
        repository.Verify(r => r.DeleteAsync(id), Times.Once);
    }
}

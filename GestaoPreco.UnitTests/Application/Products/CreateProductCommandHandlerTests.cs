using Domain;
using FluentAssertions;
using GestaoPreco.Application.CommandHandlers.ProductHandlers;
using GestaoPreco.Application.Commands.Product;
using Infrastructure;
using Moq;

namespace GestaoPreco.UnitTests.Application.Products;

public class CreateProductCommandHandlerTests
{
    [Fact]
    public async Task Handle_Deve_Persistir_E_Retornar_Novo_Id()
    {
        var command = new CreateProductCommand
        {
            Name = "Monitor",
            Price = 800m
        };

        var repository = new Mock<IProductRepository>();
        repository.Setup(r => r.AddAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

        var handler = new CreateProductCommandHandler(repository.Object);

        var id = await handler.Handle(command, default);

        id.Should().NotBeEmpty();
        repository.Verify(
            r => r.AddAsync(It.Is<Product>(p => p.Name == "Monitor" && p.Price == 800m)),
            Times.Once);
    }
}

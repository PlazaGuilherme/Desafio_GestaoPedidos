using Application.Queries.Product;
using Application.QueryHandlers.Product;
using Domain;
using FluentAssertions;
using Infrastructure;
using Moq;

namespace GestaoPreco.UnitTests.Application.Products;

public class ListProductsQueryHandlerTests
{
    [Fact]
    public async Task Handle_Deve_Retornar_Produtos_Do_Repositorio()
    {
        var products = new List<Product>
        {
            new()
            {
                Name = "Teclado",
                Price = 120m
            }
        };

        var repository = new Mock<IProductRepository>();
        repository.Setup(r => r.GetAllAsync()).ReturnsAsync(products);

        var handler = new ListProductsQueryHandler(repository.Object);

        var result = await handler.Handle(new ListProductsQuery(), default);

        result.Should().BeEquivalentTo(products);
        repository.Verify(r => r.GetAllAsync(), Times.Once);
    }
}

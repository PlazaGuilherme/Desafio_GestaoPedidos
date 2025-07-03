using MediatR;

namespace GestaoPreco.Application.Commands.Product
{
    public class CreateProductCommand : IRequest<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
namespace GestaoPreco.Application.Commands.Product
{
    public class CreateProductCommand
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
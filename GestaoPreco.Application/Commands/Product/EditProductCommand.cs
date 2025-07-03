using System;

namespace GestaoPreco.Application.Commands.Product
{
    public class EditProductCommand 
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using GestaoPreco.Application.Commands.Product;
using Infrastructure;

namespace GestaoPreco.Application.CommandHandlers.ProductHandlers
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly IProductRepository _repository;

        public UpdateProductCommandHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(request.Id);
            if (product == null)
                return false;

            product.Name = request.Name;
            product.Price = request.Price;

            await _repository.UpdateAsync(product);
            return true;
        }
    }
}
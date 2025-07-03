using MediatR;
using GestaoPreco.Application.Commands.Product;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly IProductRepository _repository;

    public DeleteProductCommandHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(request.Id);
        if (product == null)
            return false;

        await _repository.DeleteAsync(request.Id);
        return true;
    }
}
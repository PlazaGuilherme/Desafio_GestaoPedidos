using MediatR;
using GestaoPreco.Application.Queries.Customer;
using Domain;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure;

public class ListCustomersQueryHandler : IRequestHandler<ListCustomersQuery, IEnumerable<Customer>>
{
    private readonly ICustomerRepository _customerRepository;

    public ListCustomersQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<IEnumerable<Customer>> Handle(ListCustomersQuery request, CancellationToken cancellationToken)
    {
        return await _customerRepository.GetAllAsync();
    }
}
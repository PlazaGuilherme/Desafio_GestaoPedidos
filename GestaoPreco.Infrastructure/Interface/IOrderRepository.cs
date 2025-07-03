using Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order);
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(Guid id);
        Task UpdateAsync(Order order);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<Order>> GetAllWithItemsAsync();
        Task<Order?> GetByIdWithItemsAsync(Guid id);
    }
}
using Domain;

namespace Infrastructure
{
    public interface IOrderItemRepository
    {
        Task AddAsync(OrderItem orderItem);
        Task<IEnumerable<OrderItem>> GetAllAsync();
        Task<OrderItem?> GetByIdAsync(Guid id);
        Task UpdateAsync(OrderItem orderItem);
        Task DeleteAsync(Guid id);
    }
}
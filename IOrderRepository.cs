public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetAllWithItemsAsync();
}
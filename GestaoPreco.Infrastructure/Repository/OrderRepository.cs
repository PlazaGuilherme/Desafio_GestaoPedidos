using Domain;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;


namespace GestaoPedido.Infrastructure.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;
        private readonly IMongoCollection<Order> _mongoOrders;

        public OrderRepository(AppDbContext context, IMongoCollection<Order> mongoOrders)
        {
            _context = context;
            _mongoOrders = mongoOrders;
        }

        public async Task AddAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            await _mongoOrders.InsertOneAsync(order);
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _mongoOrders.Find(_ => true).ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetAllWithItemsAsync()
        {
            return await _mongoOrders.Find(_ => true).ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(Guid id)
        {
            return await _mongoOrders.Find(o => o.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Order?> GetByIdWithItemsAsync(Guid id)
        {
            return await _mongoOrders.Find(o => o.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(Order order)
        {
            await _mongoOrders.ReplaceOneAsync(oi => oi.Id == order.Id, order);

            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            await _mongoOrders.DeleteOneAsync(oi => oi.Id == id);
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }
    }
}
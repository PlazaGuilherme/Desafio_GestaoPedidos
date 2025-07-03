using Domain;
using MongoDB.Driver;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure;

namespace GestaoPedido.Infrastructure.Repository
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly IMongoCollection<OrderItem> _mongoOrderItems;
        private readonly AppDbContext _context;

        public OrderItemRepository(IMongoCollection<OrderItem> mongoOrderItems, AppDbContext context)
        {
            _mongoOrderItems = mongoOrderItems;
            _context = context;
        }

        public async Task AddAsync(OrderItem orderItem)
        {
            _context.OrderItens.Add(orderItem);
            await _mongoOrderItems.InsertOneAsync(orderItem);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<OrderItem>> GetAllAsync()
        {
            return await _mongoOrderItems.Find(_ => true).ToListAsync();
        }

        public async Task<OrderItem?> GetByIdAsync(Guid id)
        {
            return await _mongoOrderItems.Find(oi => oi.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(OrderItem orderItem)
        {
            await _mongoOrderItems.ReplaceOneAsync(oi => oi.Id == orderItem.Id, orderItem);

            _context.OrderItens.Update(orderItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            await _mongoOrderItems.DeleteOneAsync(oi => oi.Id == id);

            var orderItem = await _context.OrderItens.FindAsync(id);
            if (orderItem != null)
            {
                _context.OrderItens.Remove(orderItem);
                await _context.SaveChangesAsync();
            }
        }
    }
}
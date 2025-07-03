using Domain;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly AppDbContext _context;

        public OrderItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(OrderItem orderItem)
        {
            _context.OrderItens.Add(orderItem);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<OrderItem>> GetAllAsync()
        {
            return await _context.OrderItens.ToListAsync();
        }

        public async Task<OrderItem?> GetByIdAsync(Guid id)
        {
            return await _context.OrderItens.FindAsync(id);
        }

        public async Task UpdateAsync(OrderItem orderItem)
        {
            _context.OrderItens.Update(orderItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var orderItem = await _context.OrderItens.FindAsync(id);
            if (orderItem != null)
            {
                _context.OrderItens.Remove(orderItem);
                await _context.SaveChangesAsync();
            }
        }
    }
}
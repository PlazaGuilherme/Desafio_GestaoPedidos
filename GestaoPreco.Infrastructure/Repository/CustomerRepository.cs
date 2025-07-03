using Domain;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestaoPedido.Infrastructure.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;
        private readonly IMongoCollection<Customer> _mongoCustomers;

        public CustomerRepository(AppDbContext context, IMongoCollection<Customer> mongoCustomers)
        {
            _context = context;
            _mongoCustomers = mongoCustomers;
        }

        public async Task AddAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            await _mongoCustomers.InsertOneAsync(customer);
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _mongoCustomers.Find(_ => true).ToListAsync();
        }

        public async Task<Customer?> GetByIdAsync(Guid id)
        {
            return await _mongoCustomers.Find(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();

            await _mongoCustomers.ReplaceOneAsync(c => c.Id == customer.Id, customer);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _mongoCustomers.DeleteOneAsync(c => c.Id == id);

            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }
    }
}
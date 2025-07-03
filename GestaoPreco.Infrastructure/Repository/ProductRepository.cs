using Domain;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestaoPedido.Infrastructure.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        private readonly IMongoCollection<Product> _mongoProducts;

        public ProductRepository(AppDbContext context, IMongoCollection<Product> mongoProducts)
        {
            _context = context;
            _mongoProducts = mongoProducts;
        }

        public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            await _mongoProducts.InsertOneAsync(product);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _mongoProducts.Find(_ => true).ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await _mongoProducts.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);

            await _context.SaveChangesAsync();
            await _mongoProducts.ReplaceOneAsync(p => p.Id == product.Id, product);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _mongoProducts.DeleteOneAsync(p => p.Id == id);

            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VStoreAPI.Models;
using VStoreAPI.Services;

namespace VStoreAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<Product>> GetAsync()
        {
            var products = await _context
                .Products
                .AsNoTracking()
                .ToListAsync();
            
            foreach (var product in products)
                product.Order = await _context
                    .Orders
                    .FirstOrDefaultAsync(x => x.Id == product.OrderId);
            
            return products;
        }

        public async Task<Product> GetAsync(int id)
        {
            var product = await _context
                .Products
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            
            product.Order = await _context
                .Orders
                .FirstOrDefaultAsync(x => x.Id == product.OrderId);
            
            return product;
        }

        public async Task<Product> CreateAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task Delete(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VStoreAPI.Models;
using VStoreAPI.Services;

namespace VStoreAPI.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;
        public OrderRepository(AppDbContext context) => _context = context;
        
        public async Task<IEnumerable<Order>> GetAsync()
        {
            var orders = await _context
                .Orders
                .AsNoTracking()
                .ToListAsync();

            foreach (var order in orders)
            {
                order.Products = await _context
                    .Products
                    .Where(x => x.OrderId == order.Id)
                    .ToListAsync();
                order.User = await _context
                    .Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == order.UserId);
            }
                
            
            
            return orders;
        }
        public async Task<IEnumerable<Order>> GetAsyncByUserId(int id)
        {
            return await _context
                .Orders
                .AsNoTracking()
                .Where(x => x.UserId == id)
                .ToListAsync();
        }

        public async Task<Order> GetAsync(int id)
        {
            var order = await _context
                .Orders
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            
            order.User = await _context
                .Users
                .FirstOrDefaultAsync(x => x.Id == order.UserId);
            
            order.Products = await _context.Products
                .Where(x => x.OrderId == order.Id)
                .ToListAsync();

            return order;
        }

        public async Task<Order> CreateAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task Delete(Order order)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Order order)
        {
            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}

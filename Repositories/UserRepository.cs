using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VStoreAPI.Models;
using VStoreAPI.Services;
using System.Linq;

namespace VStoreAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context) => _context = context;
    
        public async Task<IEnumerable<User>> GetAsync()
        {
            var users = await _context.Users.ToListAsync();
            foreach (var user in users)
                user.Orders = await _context.Orders
                    .Where(x => x.UserId == user.Id)
                    .ToListAsync();
            return users;
        }

        public async Task<User> GetAsync(int id)
        {
             var user = await _context
                .Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
             
             user.Orders = await _context.Orders
                 .Where(x => x.UserId == user.Id)
                 .ToListAsync();
             
             return user;
        }

        public async Task<User> LoginAsync(string email, string password)
        {
            return await _context
                            .Users
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Email == email && x.Password == password);
        }

        public async Task<User> CreateAsync(User user)
        {
            await _context.Users.AddAsync(user);
            user.Orders = new List<Order>();
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task Delete(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task Update(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}

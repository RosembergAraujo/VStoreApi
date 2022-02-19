using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VStoreAPI.Models;
using VStoreAPI.Services;

namespace VStoreAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context) => _context = context;
    
        public async Task<IEnumerable<User>> GetAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetAsync(int id)
        {
            return await _context
                .Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
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
            await _context.SaveChangesAsync();
            return user;
        }

        public Task Delete(User user)
        {
            throw new System.NotImplementedException();
        }

        public Task Update(User user)
        {
            throw new System.NotImplementedException();
        }
    }
}

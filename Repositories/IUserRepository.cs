using System.Collections.Generic;
using System.Threading.Tasks;
using VStoreAPI.Models;

namespace VStoreAPI.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAsync(); 
        
        Task<User> GetAsync(int id);
        
        Task<User> LoginAsync(string email, string password);
            
        Task<User> CreateAsync(User user);
        
        Task Delete(User user);

        Task Update(User user);
    }
}

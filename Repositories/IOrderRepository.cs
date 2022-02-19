using System.Collections.Generic;
using System.Threading.Tasks;
using VStoreAPI.Models;

namespace VStoreAPI.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAsyncByUserId(int userId); //Temp
        
        Task<Order> GetAsync(int id);
            
        Task<Order> CreateAsync(Order order);
        
        Task Delete(Order order);

        Task Update(Order order);
    }
}

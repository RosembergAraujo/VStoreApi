using System.Collections.Generic;
using System.Threading.Tasks;
using VStoreAPI.Models;

namespace VStoreAPI.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAsyncByUserId(int userId); //Temp
        
        Task<Product> GetAsync(int id);
            
        Task<Product> CreateAsync(Product product);
        
        Task Delete(Product product);

        Task Update(Product product);
    }
}

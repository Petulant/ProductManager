using ProductManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManager.Domain.Interface
{
    public interface ICategoryRepository
    {
        // Returns null if the ID isn't found
        Task<Category?> GetByIdAsync(long id);
        Task<IEnumerable<Category>> GetByUserIdAsync(long userId);
        Task AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(long id);
        Task<List<long>> GetAllIdsAsync();
        Task<bool> ExistsByCodeAsync(string categoryCode);
    }
}

using ProductManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManager.Domain.Interface
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(long id);
        Task<IEnumerable<Product>> GetByUserIdAsync(long userId);   
        Task AddAsync(Product product);
        Task AddRangeAsync(IEnumerable<Product> products);
        Task UpdateAsync(Product product);
        Task DeleteAsync(long id);
        Task DeleteRangeAsync(IEnumerable<long> ids);
        Task<IEnumerable<Product>> GetPagedAsync(int pageNumber, int pageSize);
        Task<int> GetCountByMonthAsync(string yearMonth);
    }
}

using ProductManager.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManager.Application.Interface
{
    public interface IProductService
    {
        Task<ProductDto?> GetByIdAsync(long id);
        Task<IEnumerable<ProductDto>> GetPagedAsync(int pageNumber);
        Task AddAsync(ProductDto productDto);
        Task AddMultipleProductsAsync(IEnumerable<ProductDto> dtos);
        Task<bool> UpdateAsync(ProductDto productDto);
        Task<bool> DeleteAsync(long id);
        Task DeleteRangeAsync(IEnumerable<long> ids);
   
    }
}

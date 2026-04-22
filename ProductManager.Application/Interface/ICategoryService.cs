using ProductManager.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManager.Application.Interface
{
    public interface ICategoryService
    {     
        Task<CategoryDTO?> GetByIdAsync(long id);
        Task<IEnumerable<CategoryDTO>> GetByUserIdAsync(long userId);
        Task<CategoryDTO> AddAsync(CategoryDTO categoryDto);
        Task<bool> UpdateAsync(CategoryDTO categoryDto);
        Task DeleteAsync(long id);
      

    }
}

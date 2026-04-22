using Microsoft.EntityFrameworkCore;
using ProductManager.Domain.Interface;
using ProductManager.Domain.Models;
using ProductManager.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManager.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ProductManagerContext _context;

        public CategoryRepository(ProductManagerContext context)
        {
            _context = context;
        }

        public async Task<Category?> GetByIdAsync(long id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<IEnumerable<Category>> GetByUserIdAsync(long userId)
        {
            return await _context.Categories
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }
        public async Task AddAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(long id)
        {
            var removeCategory = await _context.Categories.FindAsync(id);
            if (removeCategory != null)
            {
                _context.Categories.Remove(removeCategory);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<long>> GetAllIdsAsync()
        {   
            return await _context.Categories.Select(c => c.CategoryId).ToListAsync();
        }

        public async Task<bool> ExistsByCodeAsync(string categoryCode)
        {
            return await _context.Categories.AnyAsync(c => c.CategoryCode == categoryCode);
        }
    }
}

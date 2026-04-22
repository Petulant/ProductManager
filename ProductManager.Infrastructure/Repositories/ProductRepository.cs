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
    public class ProductRepository : IProductRepository
    {
        private readonly ProductManagerContext _context;

        public ProductRepository(ProductManagerContext context)
        {
            _context = context;
        }

        public async Task<Product?> GetByIdAsync(long id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<IEnumerable<Product>> GetByUserIdAsync(long userId)
        {
            return await _context.Products
              .Include(p => p.Category)
              .Where(p => p.Category.UserId == userId)
              .ToListAsync();
        }
        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }
        public async Task AddRangeAsync(IEnumerable<Product> products)
        {
            await _context.Products.AddRangeAsync(products);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(long id)
        {
            var removeProduct = await _context.Products.FindAsync(id);
            if (removeProduct != null)
            {
                _context.Products.Remove(removeProduct);
                await _context.SaveChangesAsync();
            }
        }
        public async Task DeleteRangeAsync(IEnumerable<long> ids)
        {
            var productsToDelete = await _context.Products
                .Where(p => ids.Contains(p.ProductId))
                .ToListAsync();

            if (productsToDelete.Any())
            {
                _context.Products.RemoveRange(productsToDelete);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Product>> GetPagedAsync(int pageNumber, int pageSize)
        {
            return await _context.Products
                .OrderBy(p => p.Name)
                .Skip((pageNumber - 1) * pageSize) 
                .Take(pageSize)                
                .ToListAsync();
        }
        public async Task<int> GetCountByMonthAsync(string yearMonth)
        {
            return await _context.Products
                .CountAsync(p => p.ProductCode.StartsWith(yearMonth));
        }
    }
}

using ProductManager.Application.DTOs;
using ProductManager.Application.Interface;
using ProductManager.Domain.Interface;
using ProductManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManager.Application.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // methods Mapping DTOs to Entities/Models

        public async Task<CategoryDTO?> GetByIdAsync(long id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) { return null; }

            return new CategoryDTO
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                CategoryCode = category.CategoryCode,
                IsActive = category.IsActive,
            };

        }
        public async Task<IEnumerable<CategoryDTO>> GetByUserIdAsync(long userId)
        {
            var categories = await _categoryRepository.GetByUserIdAsync(userId);
            return categories.Select(c => new CategoryDTO
            {
                CategoryId = c.CategoryId,
                Name = c.Name,
                CategoryCode = c.CategoryCode,
                IsActive = c.IsActive
            });

        }
        public async Task<CategoryDTO> AddAsync(CategoryDTO categoryDto)
        {
            bool exists = await _categoryRepository.ExistsByCodeAsync(categoryDto.CategoryCode);

            if (exists)
            {
                throw new InvalidOperationException("Category Code already exists. Please use a unique code.");
            }
            var category = new Category
            {
                Name = categoryDto.Name,
                CategoryCode = categoryDto.CategoryCode,
                //UserId = 1,
                IsActive = categoryDto.IsActive
            };
            await _categoryRepository.AddAsync(category);
            categoryDto.CategoryId = category.CategoryId;
            return categoryDto;

        }
        public async Task<bool> UpdateAsync(CategoryDTO category)
        {
            var existingCategory = await _categoryRepository.GetByIdAsync(category.CategoryId);

            if (existingCategory == null)
                return false;

            existingCategory.Name = category.Name;
            existingCategory.CategoryCode = category.CategoryCode;
            existingCategory.IsActive = category.IsActive;

           await _categoryRepository.UpdateAsync(existingCategory);
            return true;           
        }
        public async Task DeleteAsync(long id)
        {
            var entity = await _categoryRepository.GetByIdAsync(id);
            if (entity != null)
            {
                await _categoryRepository.DeleteAsync(entity.CategoryId);
            }
        }

    }
}

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
    public class ProductService : IProductService
    {
        // Inject the Repository Interface
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }
        public async Task<ProductDto?> GetByIdAsync(long id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) { return null; }

            return new ProductDto
            {
                ProductId = product.ProductId,
                ProductCode = product.ProductCode,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Image = product.Image,
              
            };

        }
        public async Task AddAsync(ProductDto productDto)
        {
            string datePart = DateTime.Now.ToString("yyyyMM"); 

            int currentMonthCount = await _productRepository.GetCountByMonthAsync(datePart);

            string sequencePart = (currentMonthCount + 1).ToString("D3");
            var products = new Product
            {
                ProductId = productDto.ProductId,
                ProductCode = $"{datePart}-{sequencePart}",
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Image = productDto.ImagePath,
                CategoryId = productDto.CategoryId // Mapping the FK
            };
            await _productRepository.AddAsync(products);

            productDto.ProductId = products.ProductId;
            productDto.ProductCode = products.ProductCode;
     
        }

        public async Task AddMultipleProductsAsync(IEnumerable<ProductDto> dtos)
        {
            var validCategoryIds = await _categoryRepository.GetAllIdsAsync();

            var products = new List<Product>();

            foreach (var dto in dtos)
            {
                if (!validCategoryIds.Contains(dto.CategoryId))
                {
                    throw new Exception($"Category ID {dto.CategoryId} does not exist.");
                }
                products.Add(new Product
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    Price = dto.Price,
                    ProductCode = dto.ProductCode,
                    Image = dto.Image,
                    CategoryId = dto.CategoryId,    // Mapping the FK

                });
            }

            await _productRepository.AddRangeAsync(products);
        }

        public async Task<bool> UpdateAsync(ProductDto productDto)
        {
            var existingProduct = await _productRepository.GetByIdAsync(productDto.ProductId);

            if (existingProduct == null) 
                return false;

                existingProduct.ProductCode = productDto.ProductCode;
                existingProduct.Name = productDto.Name;
                existingProduct.Description = productDto.Description;
                existingProduct.Price = productDto.Price;
                existingProduct.Image = productDto.Image;
            
                await _productRepository.UpdateAsync(existingProduct);
                return true;
        }
        public async Task<bool> DeleteAsync(long id)
        {
            var removeProduct = await _productRepository.GetByIdAsync(id);
            if (removeProduct == null) 
                return false;

            await _productRepository.DeleteAsync(id);
            return true;

        }
        public async Task DeleteRangeAsync(IEnumerable<long> ids)
        {
            await _productRepository.DeleteRangeAsync(ids); ;
        }

        public async Task<IEnumerable<ProductDto>> GetPagedAsync(int pageNumber)
        {
            int pageSize = 10;

            var products = await _productRepository.GetPagedAsync(pageNumber, pageSize);
            return products.Select(p => new ProductDto
            {
                ProductId = p.ProductId,
                ProductCode = p.ProductCode,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Image = p.Image,
            }).ToList();
        }

   
    }
        
}

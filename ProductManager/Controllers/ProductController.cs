using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniExcelLibs;
using ProductManager.Application.DTOs;
using ProductManager.Application.Interface;
using ProductManager.Application.Service;
using ProductManager.Domain.Models;

namespace ProductManager.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        // Add a Single Product
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductDto productDto)
        {
            if (productDto.ImageFile != null && productDto.ImageFile.Length > 0)
            {
          
                string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images"); // Define where to save (e.g., wwwroot/images)
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(productDto.ImageFile.FileName);    //  Generate unique filename
                string filePath = Path.Combine(folder, fileName);

               
                using (var stream = new FileStream(filePath, FileMode.Create)) // Save the file to the folder
                {
                    await productDto.ImageFile.CopyToAsync(stream);
                }
            
                productDto.ImagePath = "/images/" + fileName;  //Save the relative path for the DB
            }
            
            await _productService.AddAsync(productDto);
            return Ok();
        }

        // Add multiple Products
        [HttpPost("bulk")]
        public async Task<IActionResult> CreateMultipleProducts([FromBody] List<ProductDto> productDtos)
        {
            if (productDtos == null || !productDtos.Any())
                return BadRequest("No products provided.");

            await _productService.AddMultipleProductsAsync(productDtos);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] ProductDto productDto)
        {
            if (id != productDto.ProductId) 
                return BadRequest("ID mismatch");

            var result = await _productService.UpdateAsync(productDto);

            if (!result) 
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var deleted = await _productService.DeleteAsync(id);
            if (!deleted) 
                return NotFound($"Product with ID {id} not found.");

            return NoContent();
        }

        [HttpPost("bulk-delete")]
        public async Task<IActionResult> DeleteMultiple([FromBody] IEnumerable<long> ids)
        {
            if (ids == null || !ids.Any()) return BadRequest("No IDs provided.");

            await _productService.DeleteRangeAsync(ids);
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1)
        {
            await _productService.GetPagedAsync(pageNumber);
            return Ok();
        }
       
        [HttpPost("upload-excel")]
        [AllowAnonymous]
        public async Task<IActionResult> UploadExcel(IFormFile excelFile)
        {
               if (excelFile == null || excelFile.Length <= 0)
                  return BadRequest("Please upload a valid Excel file.");

                if (!Path.GetExtension(excelFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                    return BadRequest("Only .xlsx files are supported.");

                try
                {
                    using (var stream = excelFile.OpenReadStream())
                    {
                        // 1. Read Excel rows directly into a list of DTOs
                        var rows = stream.Query<ExcelProductDto>().ToList();

                        // 2. Loop and save to database
                        foreach (var row in rows)
                        {
                            var product = new ProductDto
                            {
                                CategoryId = row.CategoryId,
                                Name = row.Name,
                                Price = row.Price,
                                ProductCode = row.ProductCode

                            };

                          await _productService.AddAsync(product);
                    }
                }

                   return Ok(new { message = "Products imported successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

}
}

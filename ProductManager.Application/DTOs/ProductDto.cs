using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManager.Application.DTOs
{
    public class ProductDto
    {
        public long ProductId { get; set; }
        public required string? ProductCode { get; set; }
        public required string? Name { get; set; }
        public string? Description { get; set; }
        public required decimal Price { get; set; }
        public string? Image { get; set; }
        public long CategoryId { get; set; }

        public IFormFile? ImageFile { get; set; }
        public string? ImagePath{ get; set; }
    }
}

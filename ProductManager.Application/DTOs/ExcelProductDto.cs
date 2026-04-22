using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManager.Application.DTOs
{
    public class ExcelProductDto
    {
        
        public string? ProductCode { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public long CategoryId { get; set; }
  
    }

}

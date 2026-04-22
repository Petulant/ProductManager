using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManager.Application.DTOs
{
    public class CreateCategoryDTO
    {
        public long CategoryId { get; set; }
        public required string Name { get; set; }
        public required string CategoryCode { get; set; }
        public bool IsActive { get; set; }
    }
}

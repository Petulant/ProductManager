using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManager.Application.DTOs
{
    public class CategoryDTO
    {
        public long CategoryId { get; set; }
        public required string Name { get; set; }
        [Required]
        [RegularExpression(@"^[A-Z]{3}\d{3}$", ErrorMessage = "Category Code must be 3 uppercase letters followed by 3 numbers (e.g., ABC123)")]
        public required string CategoryCode { get; set; }
        public bool IsActive { get; set; }
    }
}

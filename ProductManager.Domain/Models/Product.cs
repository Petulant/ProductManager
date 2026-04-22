using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManager.Domain.Models
{
    public class Product
    {
        [Key]
        public long ProductId { get; set; }
        public required string ProductCode { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required decimal Price { get; set; }
        public string? Image { get; set; }


        // --- Foreign Key Setup ---
        public long CategoryId { get; set; }
        public Category? Category { get; set; }


        public string CreatedBy { get; set; } = "System"; // Or the logged-in username
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
    
}

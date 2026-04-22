using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManager.Domain.Models
{
    public class Category
    {
        [Key]
        public long CategoryId { get; set; }
        public required string Name { get;set; }
        public required string CategoryCode { get;set; }
        public bool IsActive { get;set; }

        // ownership
        public long UserId { get; set; }

        // Relationship: One Category has Many Products
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ProductsCategories.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Category name cannot be blank")]
        public string Name { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public List<Association> ProductsInCategory { get; set; }

    }
}
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ProductsCategories.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product name cannot be blank")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Product description cannot be blank")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Product price cannot be blank")]
        public decimal Price { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public List<Association> CategoriesOfProduct { get; set; }
    }
}
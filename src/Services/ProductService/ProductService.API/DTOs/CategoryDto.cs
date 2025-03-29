using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductService.API.DTOs
{
    public class CategoryDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool IsFeatured { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
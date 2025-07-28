using System;
using System.ComponentModel.DataAnnotations;

namespace SimpleApi.Models.DTOS
{
    public class ProductDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
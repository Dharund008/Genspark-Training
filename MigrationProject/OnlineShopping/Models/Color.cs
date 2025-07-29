using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online.Models
{
    public class Color
    {
         [Key]
        public int ColorId { get; set; }

        [Required]
        public string ColorName { get; set; } = string.Empty;

        public ICollection<Product>? Products { get; set; }
    }
   
}
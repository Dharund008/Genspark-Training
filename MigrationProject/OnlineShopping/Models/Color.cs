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
        public string Color1 { get; set; }

        public ICollection<Product> Products { get; set; }
    }
   
}
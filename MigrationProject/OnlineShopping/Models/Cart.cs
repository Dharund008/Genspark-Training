using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }

        [Required]
        public int ProductId { get; set; }
        
        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }

        public int Quantity { get; set; }
    }
}
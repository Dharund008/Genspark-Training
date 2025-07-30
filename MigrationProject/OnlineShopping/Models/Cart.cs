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

        [Required]
        public int UserId { get; set; }
        public int Quantity { get; set; }
        
        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }
        public User? User{ get; set; }
       
    }
}
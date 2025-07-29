using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public string OrderName { get; set; } = string.Empty;

        public DateTime? OrderDate { get; set; }

        [Required]
        public string PaymentType { get; set; } = string.Empty;
        [Required]
        public string Status { get; set; } = string.Empty;

        //properties
        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }
    
}
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Online.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public double TotalAmount { get; set; }

        public int UserId{ get; set; }

        public DateTime? OrderDate { get; set; } = DateTime.UtcNow;

        //[Required]
        //public string PaymentType { get; set; } = string.Empty;
        [Required]
        public string Status { get; set; } = string.Empty;

        //properties
        [JsonIgnore]
        public User? User { get; set; }
        
        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }
    
}
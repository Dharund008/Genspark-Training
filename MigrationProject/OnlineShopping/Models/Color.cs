using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Online.Models
{
    public class Color
    {
         [Key]
        public int ColorId { get; set; }

        [Required]
        public string ColorName { get; set; } = string.Empty;

        [JsonIgnore]
        public ICollection<Product>? Products { get; set; }
    }
   
}
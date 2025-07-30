using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Online.Models
{
    public class Model
    {
        [Key]
        public int ModelId { get; set; }

        [Required]
        public string ModelName { get; set; } = string.Empty;

        [JsonIgnore]
        public ICollection<Product>? Products { get; set; }
    }
   
}
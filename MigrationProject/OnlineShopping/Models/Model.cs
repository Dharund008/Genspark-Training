using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online.Models
{
    public class Model
    {
        [Key]
        public int ModelId { get; set; }

        [Required]
        public string ModelName { get; set; } = string.Empty;

        public ICollection<Product>? Products { get; set; }
    }
   
}
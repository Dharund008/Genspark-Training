using System.ComponentModel.DataAnnotations;
using Online.Models;

namespace Online.Models.DTO
{
    public class AddprdDTO
    {
        [Required]
        public string ProductName { get; set; } = string.Empty;

        public string? Image { get; set; }
        public double Price { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        // public DateTime? SellStartDate { get; set; }
        // public DateTime? SellEndDate { get; set; }

    }

    public class PriceDTO
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public double Price { get; set; }
    }
    public class ExtendSaleDTO
    {
        public int ProductId { get; set; }
        public DateTime NewEndDate { get; set; }
    }


    public class SaleDTO
    {
        [Required]
        public int ProductId { get; set; }
        public DateTime? SellStartDate { get; set; }
        public DateTime? SellEndDate { get; set; }
    }

    public class UpdateProductDTO
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public string ProductName { get; set; } = string.Empty;
        public string? Image { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
    }
}
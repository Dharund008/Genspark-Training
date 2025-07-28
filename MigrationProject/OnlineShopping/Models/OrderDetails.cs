using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online.Models
{
    public class OrderDetail
    {
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    
        //properties
        public Order? Order { get; set; }
        public Product? Product { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ECommerceSystem.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; } 
        public ICollection<OrderDetail> orderDetails { get; set; }

        
    }
}

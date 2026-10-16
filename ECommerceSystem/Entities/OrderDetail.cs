using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceSystem.Entities
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId  { get; set; }
        public Order Order { get; set; } = null!;

        public int Quantity { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

    }
}

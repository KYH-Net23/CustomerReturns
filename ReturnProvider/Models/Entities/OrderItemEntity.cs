using System;

namespace ReturnProvider.Models.Entities
{
    public class OrderItemEntity
    {
        public Guid ItemId { get; set; }
        public Guid OrderId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}

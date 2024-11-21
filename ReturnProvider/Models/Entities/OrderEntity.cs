using System;
using System.Collections.Generic;

namespace ReturnProvider.Models.Entities
{
    public class OrderEntity
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public bool IsReturnable { get; set; }
        public ICollection<OrderItemEntity> Items { get; set; }
    }
}

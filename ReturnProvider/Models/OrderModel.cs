namespace ReturnProvider.Models
{
    public class OrderModel
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public bool IsReturnable { get; set; }
        public List<OrderItemModel> Items { get; set; }
    }

}

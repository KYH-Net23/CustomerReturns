using Microsoft.EntityFrameworkCore;
using ReturnProvider.Models;

namespace ReturnProvider.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderModel>> GetReturnableOrdersAsync(Guid userId)
        {
            var orders = await _context.Orders
                .Where(o => o.UserId == userId && o.IsReturnable)
                .Include(o => o.Items) 
                .Select(o => new OrderModel
                {
                    OrderId = o.OrderId,
                    UserId = o.UserId,
                    IsReturnable = o.IsReturnable,
                    Items = o.Items.Select(i => new OrderItemModel
                    {
                        ItemId = i.ItemId,
                        Name = i.Name,
                        Price = i.Price,
                        Quantity = i.Quantity
                    }).ToList()
                })
                .ToListAsync();

            return orders;
        }

        public async Task<OrderModel?> GetOrderByIdAsync(Guid orderId)
        {
            var order = await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null) return null;

            return new OrderModel
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                IsReturnable = order.IsReturnable,
                Items = order.Items.Select(item => new OrderItemModel
                {
                    ItemId = item.ItemId,
                    Name = item.Name,
                    Price = item.Price,
                    Quantity = item.Quantity
                }).ToList()
            };
        }

    }
}

using ReturnProvider.Models;

namespace ReturnProvider.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<OrderModel>> GetReturnableOrdersAsync(Guid userId);
        Task<OrderModel?> GetOrderByIdAsync(Guid orderId);
    }
}

using OrderSystem.Models;

namespace OrderSystem.Repository
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetOrdersAsync();
        Task<Order> GetOrderByIdAsync(int id);
    }
}

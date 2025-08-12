using OrderSystem.Models;

namespace OrderSystem.Repository
{
    public interface IProductsRepository
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
    }
}

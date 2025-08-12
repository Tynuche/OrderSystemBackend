using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using OrderSystem.Models;

namespace OrderSystem.Repository
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly OrderSysDBContext _context;

        public ProductsRepository(OrderSysDBContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            var products = await _context.Products.Select(p => new Product
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                Description = p.Description,
                Price = p.Price,
                Inventory = p.Inventory,
            }).ToListAsync();

            return products;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            return product;
        }
    }
}

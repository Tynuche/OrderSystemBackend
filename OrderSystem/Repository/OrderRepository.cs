using Microsoft.EntityFrameworkCore;
using OrderSystem.Models;

namespace OrderSystem.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderSysDBContext _context;

        public OrderRepository(OrderSysDBContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            var orders = await _context.Orders.Select(o => new Order
            {
                OrderId = o.OrderId,
                UserId = o.UserId,
                OrderDate = o.OrderDate,
                Status = o.Status,
                OrderItems = o.OrderItems.Select(oi => new OrderItem
                {
                    OrderItemId = oi.OrderItemId,
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    Product = new Product
                    {
                        ProductId = oi.Product.ProductId,
                        ProductName = oi.Product.ProductName,
                        Description = oi.Product.Description,
                        Price = oi.Product.Price,
                        Inventory = oi.Product.Inventory
                    }
                }).ToList()
            }).ToListAsync();

            return orders;
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            var orders = await _context.Orders.Where(o => o.OrderId == id)
                .Select(o => new Order
                {
                    OrderId = o.OrderId,
                    UserId = o.UserId,
                    OrderDate = o.OrderDate,
                    Status = o.Status,
                    OrderItems = o.OrderItems.Select(oi => new OrderItem
                    {
                        OrderItemId = oi.OrderItemId,
                        ProductId = oi.ProductId,
                        Quantity = oi.Quantity,
                        Product = new Product
                        {
                            ProductId = oi.Product.ProductId,
                            ProductName = oi.Product.ProductName,
                            Description = oi.Product.Description,
                            Price = oi.Product.Price,
                            Inventory = oi.Product.Inventory
                        }
                    }).ToList()
                }).FirstOrDefaultAsync();

            if (orders == null)
            {
                               throw new KeyNotFoundException($"Order with ID {id} not found.");
            }
            return orders;
        }
    }
}

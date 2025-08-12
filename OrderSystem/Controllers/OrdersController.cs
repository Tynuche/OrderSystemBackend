using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderSystem.Dto;
using OrderSystem.Models;
using OrderSystem.Repository;

namespace OrderSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly OrderSysDBContext _context;

        public OrdersController(IOrderRepository orderRepository, OrderSysDBContext context)
        {
            _orderRepository = orderRepository;
            _context = context;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderRepository.GetOrdersAsync();
            return Ok(orders);
        }

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetOrderById([FromRoute]int id)
        //{
        //    var order = await _orderRepository.GetOrderByIdAsync(id);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(order);
        //}

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto orderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Create Order entity
            var order = new Order
            {
                UserId = orderDto.UserId,
                OrderDate = DateTime.UtcNow,
                Status = "Pending"
            };

            // Add OrderItems from DTO
            foreach (var itemDto in orderDto.Items)
            {
                var orderItem = new OrderItem
                {
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity
                };
                order.OrderItems.Add(orderItem);
            }

            // Save order and items (EF Core handles FK relationships)
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrderById), new { id = order.OrderId }, order);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound();

            return Ok(order);
        }
    }
}

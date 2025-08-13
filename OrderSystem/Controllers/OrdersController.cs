using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderSystem.Dto;
using OrderSystem.Models;
using OrderSystem.Repository;
using SendMessage;

namespace OrderSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly OrderSysDBContext _context;
        public IConfiguration Configuration { get; set; }
        public OrdersController(IOrderRepository orderRepository, OrderSysDBContext context, IConfiguration configuration)
        {
            _orderRepository = orderRepository;
            _context = context;
            Configuration = configuration;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderRepository.GetOrdersAsync();
            return Ok(orders);
        }
       

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto orderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var order = new Order
            {
                UserId = orderDto.UserId,
                OrderDate = DateTime.UtcNow,
                Status = "Pending"
            };

            foreach (var itemDto in orderDto.Items)
            {
                var orderItem = new OrderItem
                {
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity
                };
                order.OrderItems.Add(orderItem);
            }

            SendQueueData sendQueueData = new SendQueueData();
            string connectionString = Configuration.GetValue<string>("ConnectionStrings:ServiceBusConnection");
            string queueName = Configuration.GetValue<string>("ConnectionStrings:Queuename");
            await sendQueueData.PushDataToQueue<Order>(Task.FromResult(order), connectionString, queueName);
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

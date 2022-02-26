using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VStoreAPI.Models;
using VStoreAPI.Repositories;

namespace VStoreAPI.Controllers
{
    [ApiController]
    [Route("orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;

        
        public OrderController(
            IOrderRepository orderRepository,
            IUserRepository userRepository,
            IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
        }
        
        [HttpPost, Authorize]
        public async Task<IActionResult> PostAsync()
        {
            var tryParse = int.TryParse(User.Identity?.Name, out var userId);
            if(tryParse is false) 
                return BadRequest(new {message = "Token error"});
            
            var user = await _userRepository.GetAsync(userId);
            if (user is null)
                return NotFound(new {message = "User id not found"});
            
            var order = new Order { Status = "Empty"};
            
            user.Orders = new List<Order> {order};
        
            try
            {
                await _userRepository.Update(user);
                await _orderRepository.CreateAsync(order);
                order.User.Orders.Add(order);
                order.User.Password = string.Empty;
                user.Password = string.Empty;
                return Created($"orders/{order.Id}", new { Order = order, User = user });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
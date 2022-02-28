using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VStoreAPI.Models;
using VStoreAPI.Repositories;
using VStoreAPI.Tools;
using VStoreAPI.ViewModels;

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
        
        [HttpGet]
        public async Task<IActionResult> GetAllProductsAsync()
        {
            var orders = await _orderRepository.GetAsync();
            return orders is null ? NotFound() : Ok(new { Orders = orders });
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
                return Created($"orders/{order.Id}", new { Order = order});
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        [HttpPut, Authorize]
        public async Task<IActionResult> AddProductToOrder(AddProductToOrderViewModel model)
        {
            var tryParse = int.TryParse(User.Identity?.Name, out var tokenUserId);
            if (tryParse is false)
                return BadRequest(new { message = "Token error" });

            var order = await _orderRepository.GetAsync(model.OrderId);
            var product = await _productRepository.GetAsync(model.ProductId);
            
            if (order is null)
                return NotFound(new { message = "Order with this id not found" });
            if (product is null)
                return NotFound(new { message = "Product with this id not found" });

            if (order.UserId != tokenUserId || !AuthRoles.IsUserWithHighPrivileges(User))
                return new ObjectResult(new { message = "Cant add to this order" }) { StatusCode = 403 };

            product.OrderId = order.Id;
            product.Order = order;
            order.Products ??= new List<Product>(); //Check if is null
            order.Products.Add(product);
            order.Status = "Not Finished";

            try
            {
                await _productRepository.Update(product);
                await _orderRepository.Update(order);
                return Ok(new { Order = order, Product = product });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("int:id"), Authorize]
        public async Task<IActionResult> DeleteOrder([FromRoute] int id)
        {
            var tryParse = int.TryParse(User.Identity?.Name, out var tokenUserId);
            if (tryParse is false)
                return BadRequest(new { message = "Token error" });

            var order = await _orderRepository.GetAsync(id);
            if (order is null)
                return NotFound(new { message = "Order with this id not found" });

            if (order.UserId != tokenUserId || !AuthRoles.IsUserWithHighPrivileges(User))
                return new ObjectResult(new { message = "Cant add to this order" }) { StatusCode = 403 };

            try
            {
                await _orderRepository.Delete(order);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


    }
}
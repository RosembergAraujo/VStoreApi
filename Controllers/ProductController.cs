using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using VStoreAPI.Models;
using VStoreAPI.Repositories;
using VStoreAPI.Services;
using VStoreAPI.ViewModels;

namespace VStoreAPI.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        
        public ProductController(IProductRepository productRepository) 
            => _productRepository = productRepository;

        [HttpPost, Authorize(Roles = "admin")]
        public async Task<IActionResult> PostAsync(
            [FromBody] Product model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Wrong product model"});

            try
            {
                await _productRepository.CreateAsync(model);
                return Created($"products/{model.Id}", new { Product = model });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
    }
}
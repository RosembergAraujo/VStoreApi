using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VStoreAPI.Models;
using VStoreAPI.Repositories;
using VStoreAPI.Tools;

namespace VStoreAPI.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        
        public ProductController(IProductRepository productRepository) 
            => _productRepository = productRepository;

        [HttpGet]
        public async Task<IActionResult> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAsync();
            return products is null ? NotFound() : Ok(new { Products = products });
        }
        
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var product = await _productRepository.GetAsync(id);
            return product is null ? NotFound(new {message = "Product not found"}) : Ok( new{ Product = product } );
        }
        
        [HttpPost, Authorize]
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
        
        [HttpPut, Authorize]
        public async Task<IActionResult> PutAsync([FromBody] Product model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Wrong product model"});

            try
            {
                await _productRepository.Update(model);
                return Ok(new {Product = model});
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
                
        }
        
        [HttpDelete("{id:int}"), Authorize(Roles = "admin,dev")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var product = await _productRepository.GetAsync(id);

            if (product is null)
                return NotFound(new {message = "Product not found"});

            try
            {
                await _productRepository.Delete(product);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
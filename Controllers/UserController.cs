using System;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using VStoreAPI.Services;
using VStoreAPI.Models;
using VStoreAPI.Repositories;
using VStoreAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;

namespace VStoreAPI.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;


        public UserController(
            IUserRepository userRepository, 
            AppDbContext context, 
            [FromServices] IConfiguration config)
        {
            _config = config;
            _context = context;
            _userRepository = userRepository;
        }
        
        [HttpGet, Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllUsesAsync()
        {
            return Ok(await _userRepository.GetAsync());
        } 
        
        [HttpGet("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginUserViewModel model)
        {
            var user = await _userRepository.LoginAsync(model.Email, model.Password);
            if (user is not null)
            {
                var token = new TokenService(_config).GenerateToken(user);
                user.Password = string.Empty;
                return Ok(new { User = user, Token = token, Role = User.IsInRole("admin")});
            }
            else
            {
                return NotFound(new { message = "Wrong email or Password!" });
            }
        }
        
        [HttpGet("{id}"), Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAsync([FromRoute] int id)
        {
            var user = await _userRepository.GetAsync(id);
            return user is null? NotFound() : Ok(user);
        }
        
        [HttpPost]
        public async Task<IActionResult> PostAsync(
            [FromBody] CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Wrong user model"});
            
            var role = User.IsInRole("admin") ? "client": "admin";
            
            var user = new User
            {
                Password = model.Password,
                Email = model.Email,
                Cpf = model.Cpf,
                UserName = model.UserName,
                Birth = model.Birth,
                Phone = model.Phone,
                Gender = model.Gender,
                Date = DateTime.Now,
                Role = role
            };
            
            if (await _context.Users.FirstOrDefaultAsync(x => x.Email == model.Email) is not null) 
                return BadRequest(new { message = "This email already registred" });
            try
            {
                await _userRepository.CreateAsync(user);
                var token = new TokenService(_config).GenerateToken(user);
		        user.Password = string.Empty;
                return Created($"users/{user.Id}", new { user = user, token = token });
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPut, Authorize]
        public async Task<IActionResult> PutAsync([FromBody] CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Wrong user model"});

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == model.Email);
            
            if(User.Identity != null && user.Id.ToString() != User.Identity.Name ) 
                return Forbid();

            user.Cpf = model.Cpf;
            user.UserName = model.UserName;
            user.Birth = model.Birth;
            user.Phone = model.Phone;
            user.Gender = model.Gender;

            try
            {
                //Update From repo
                user.Password = "";
                return Ok(new {User = user});
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
                
        }
    }
}

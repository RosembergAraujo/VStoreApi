using System;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using VStoreAPI.Services;
using VStoreAPI.Models;
using VStoreAPI.Repositories;
using VStoreAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using VStoreAPI.Tools;

namespace VStoreAPI.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
            => _userRepository = userRepository;
        
        [HttpGet, Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllUsesAsync()
        {
            var users = await _userRepository.GetAsync();
            return Ok(users);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAsync([FromRoute] int id)
        {
            var tryParse = int.TryParse(User.Identity?.Name, out var tokenUserId);
            if(tryParse is false) 
                return BadRequest(new {message = "Token error"});

            if(id != tokenUserId && !_isUserWithHighPrivileges(User))
                return new ObjectResult(new {message = "Cant get this user"}) { StatusCode = 403};
            
            var user = await _userRepository.GetAsync(id);
            if (user is null)
                return NotFound();
            user.Password = string.Empty;
            return Ok(user);
        }
        
        [HttpGet("auth")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginUserViewModel model)
        {
            var user = await _userRepository.LoginAsync(model.Email, AesTool.Encrypt(model.Password));
            if (user is not null)
            {
                var token = TokenService.GenerateToken(user);
                user.Password = string.Empty;
                return Ok(new { User = user, Token = token });
            }
            else
                return NotFound(new { message = "Wrong email or Password!" });
            
        }
        
        [HttpPost]
        public async Task<IActionResult> PostAsync(
            [FromBody] CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Wrong user model"});
            
            string userRole;
            if (
                User.Identity is {IsAuthenticated: true} &&
                _isUserWithHighPrivileges(User) &&
                model.Role is not null &&
                AuthRoles.Roles.Contains(model.Role)
                )
                userRole = model.Role;
            else
                userRole = "client";

            var user = new User
            {
                Password = AesTool.Encrypt(model.Password),
                Email = model.Email,
                Cpf = model.Cpf,
                UserName = model.UserName,
                Birth = model.Birth,
                Phone = model.Phone,
                Gender = model.Gender,
                Date = DateTime.Now,
                Role = userRole
            };
            
            if (await _userRepository.LoginAsync(model.Email, AesTool.Encrypt(model.Password)) is not null) 
                return BadRequest(new { message = "This email already registered" });
            
            try
            {
                await _userRepository.CreateAsync(user);
                var token = TokenService.GenerateToken(user);
		        user.Password = string.Empty;
                return Created($"users/{user.Id}", new { user = user, token = token });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut, Authorize]
        public async Task<IActionResult> PutAsync([FromBody] CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Wrong user model"});

            var user = await _userRepository.LoginAsync(model.Email, AesTool.Encrypt(model.Password));
            if (user is null)
                return NotFound(new {message = "User Not Found"});

            if (User.Identity is null ||
                (user.Id.ToString() != User.Identity.Name &&
                 !_isUserWithHighPrivileges(User))) 
                return new ObjectResult(new {message = "Cant change this user"}) { StatusCode = 403};

            if(user.Role != model.Role && !_isUserWithHighPrivileges(User))
                return new ObjectResult(new {message = "Cant change role"}) { StatusCode = 403};
            
            user.Cpf = model.Cpf;
            user.UserName = model.UserName;
            user.Birth = model.Birth;
            user.Phone = model.Phone;
            user.Gender = model.Gender;

            try
            {
                await _userRepository.Update(user);
                user.Password = string.Empty;
                return Ok(new {User = user});
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
                
        }
        
        [HttpDelete("{id:int}"), Authorize]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!_isUserWithHighPrivileges(User) && User.Identity?.Name != id.ToString())
                return Forbid();
            
            var user = await _userRepository.GetAsync(id);

            if (user is null)
                return BadRequest(new {message = "User not found"});

            try
            {
                await _userRepository.Delete(user);
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private static bool _isUserWithHighPrivileges(IPrincipal claim) 
            => AuthRoles.HighPrivilegesRoles.Any(claim.IsInRole);

        private static bool _isUserWithLowPrivileges(IPrincipal claim) 
            => AuthRoles.LowPrivilegesRoles.Any(claim.IsInRole);
        
        private static bool _isUserInRoles(IPrincipal claim) 
            => AuthRoles.Roles.Any(claim.IsInRole);
        
    }
}

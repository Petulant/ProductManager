using Microsoft.AspNetCore.Mvc;
using ProductManager.Application.AuthDTOs;
using ProductManager.Application.Interface;

namespace ProductManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto request)
        {
            
            if (await _authService.UserExists(request.Username))//Check if user already exists
                return BadRequest("Username is already taken.");

           
            var userId = await _authService.Register(request); //Call the service to hash and save

            return Ok(new { UserId = userId, Message = "User registered successfully!" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto request)
        {
            var token = await _authService.Login(request);

            if (token == null)
                return Unauthorized("Invalid username or password.");

            return Ok(new { Token = token });
        }
    }
}

using EssenceShop.Models;
using EssenceShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace EssenceShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;

        public AuthController(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request.Username == "folarin" && request.Password == "12345")
            {
                var token = _jwtService.GenerateToken("1", request.Username, "Admin");
                return Ok(token);
            }

            return Unauthorized("Invalid username or password");
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}

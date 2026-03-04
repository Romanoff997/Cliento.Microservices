using Microsoft.AspNetCore.Mvc;

namespace Cliento.Microservices.UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        // Для простоты — регистрация возвращает новый Guid
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDto dto)
        {
            var userId = Guid.NewGuid();
            // Сохранение в БД упростили
            return Ok(new { UserId = userId, Message = "Registered (demo)" });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            // В демо — возвращаем новый/существующий GUID
            var userId = Guid.NewGuid();
            return Ok(new { UserId = userId, Message = "Logged in (demo)" });
        }
    }
}

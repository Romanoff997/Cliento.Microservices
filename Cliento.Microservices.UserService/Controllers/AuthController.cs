using Cliento.Microservices.CRMService.Data;
using Cliento.Microservices.CRMService.Models;
using Cliento.Microservices.UserService.DTOs;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cliento.Microservices.UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserCrmDbContext _db;
        public AuthController(UserCrmDbContext db)
        {
            _db = db;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            //var user = new UserCrm
            //{
            //    Id = Guid.NewGuid(),
            //    UserId = Guid.NewGuid(),
            //    Login = dto.Login,
            //    Password = dto.Password,
            //};

            //_db.Users.Add(user);
            //await _db.SaveChangesAsync();

            return Ok(new { Message = "Registered (demo)" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            //var user = await _db.Users.FirstOrDefaultAsync(x => x.Login == dto.Login && x.Password == dto.Password);
            //if (user == null) return NotFound();

            return Ok(new { userId = Guid.NewGuid(), Message = "Logged in (demo)" });
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using AvitoTestTask.Services;
using AvitoTestTask.Models;
using System.Threading.Tasks;

namespace AvitoTestTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("auth")]
        public async Task<IActionResult> Authenticate([FromBody] User request)
        {
            if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.PasswordHash))
            {
                return BadRequest(new { errors = "Необходимы имя пользователя и пароль." });
            }

            var token = await _authService.AuthenticateAsync(request.Username, request.PasswordHash);

            if (token == null)
            {
                return Unauthorized(new { errors = "Неверное имя пользователя или пароль." });
            }

            return Ok(new { token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User request)
        {
            if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.PasswordHash))
            {
                return BadRequest(new { errors = "Необходимы имя пользователя и пароль." });
            }

            var result = await _authService.RegisterAsync(request.Username, request.PasswordHash);

            if (!result)
            {
                return BadRequest(new { errors = "Пользователь с таким именем уже существует." });
            }

            return Ok(new { message = "Пользователь успешно зарегистрирован." });
        }
    }
}

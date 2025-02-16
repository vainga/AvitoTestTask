using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AvitoTestTask.Services;
using AvitoTestTask.Models;
using Microsoft.AspNetCore.Authorization;
using AvitoTestTask.Data;

namespace AvitoTestTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SendController : ControllerBase
    {
        private readonly ShopService _shopService;
        private readonly IGenericRepository<User> _userRepository;

        public SendController(ShopService shopService, IGenericRepository<User> userRepository)
        {
            _shopService = shopService;
            _userRepository = userRepository;
        }

        [HttpPost("sendCoin")]
        public async Task<IActionResult> SendCoin([FromBody] SendCoinRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.ToUser) || request.Amount <= 0)
            {
                return BadRequest(new { errors = "Некорректные данные запроса." });
            }

            var fromUserId = int.Parse(User.Identity.Name);

            var toUser = await _userRepository.FirstOrDefaultAsync(u => u.Username == request.ToUser);

            if (toUser == null)
            {
                return NotFound(new { errors = "Пользователь-получатель не найден." });
            }

            bool result = await _shopService.TransferCoins(fromUserId, toUser.Id, request.Amount);

            if (!result)
            {
                return BadRequest(new { errors = "Перевод не выполнен. Проверьте, достаточно ли у вас монет." });
            }

            return Ok(new { message = "Монеты успешно переведены." });
        }
    }

    public class SendCoinRequest
    {
        public string ToUser { get; set; }
        public int Amount { get; set; }
    }
}

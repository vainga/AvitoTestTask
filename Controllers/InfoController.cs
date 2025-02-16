using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AvitoTestTask.Data;
using AvitoTestTask.Models;
using System.Linq;

namespace AvitoTestTask.Controllers
{
    [Route("api")]
    [ApiController]
    public class InfoController : ControllerBase
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Transaction> _transactionRepository;

        public InfoController(IGenericRepository<User> userRepository, IGenericRepository<Transaction> transactionRepository)
        {
            _userRepository = userRepository;
            _transactionRepository = transactionRepository;
        }

        [HttpGet("info")]
        [Authorize]
        public async Task<IActionResult> GetInfo()
        {
            var userId = GetCurrentUserId();

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return Unauthorized(new { errors = "Пользователь не найден" });
            }

            var transactions = await _transactionRepository.GetAllAsync();
            var receivedCoins = transactions
                .Where(t => t.ToUserId == userId)
                .Select(t => new
                {
                    fromUser = t.FromUserId,
                    amount = t.Amount
                }).ToList();

            var sentCoins = transactions
                .Where(t => t.FromUserId == userId)
                .Select(t => new
                {
                    toUser = t.ToUserId,
                    amount = t.Amount
                }).ToList();

            var response = new
            {
                coins = user.Coins,
                inventory = user.Inventory.Select(i => new { type = i.Name, quantity = 1 }).ToList(),
                coinHistory = new
                {
                    received = receivedCoins,
                    sent = sentCoins
                }
            };

            return Ok(response);
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            return int.Parse(userIdClaim);
        }
    }
}

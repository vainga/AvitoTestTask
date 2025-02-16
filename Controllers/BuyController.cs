using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AvitoTestTask.Services;
using AvitoTestTask.Data;
using AvitoTestTask.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AvitoTestTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuyController : ControllerBase
    {
        private readonly ShopService _shopService;
        private readonly GenericRepository<MerchItem> _merchItemRepository;

        public BuyController(ShopService shopService, GenericRepository<MerchItem> merchItemRepository)
        {
            _shopService = shopService;
            _merchItemRepository = merchItemRepository;
        }

        [HttpGet("buy/{item}")]
        [Authorize]
        public async Task<IActionResult> BuyItem(string item)
        {
            var userId = GetUserIdFromClaim();

            var merch = await _merchItemRepository.FirstOrDefaultAsync(m => m.Name == item);

            if (merch == null)
            {
                return BadRequest(new { errors = "Товар не найден." });
            }

            var result = await _shopService.PurchaseMerch(userId, merch.Id);

            if (result)
            {
                return Ok(new { message = "Товар успешно куплен." });
            }

            return BadRequest(new { errors = "Ошибка при покупке товара." });
        }

        private int GetUserIdFromClaim()
        {
            return int.Parse(User.Claims.First(c => c.Type == "userId").Value);
        }
    }
}

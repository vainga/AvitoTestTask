using AvitoTestTask.Data;
using AvitoTestTask.Models;

namespace AvitoTestTask.Services
{
    public class ShopService
    {
        private readonly AppDbContext _context;

        public ShopService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> PurchaseMerch(int userId, int merchId)
        {
            var user = await _context.Users.FindAsync(userId);
            var merch = await _context.MerchItems.FindAsync(merchId);

            if (user == null || merch == null || user.Coins < merch.Price)
                return false;

            user.UpdateCoins(user.Coins - merch.Price); 
            user.Inventory.Add(merch);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> TransferCoins(int fromUserId, int toUserId, int amount)
        {
            if (fromUserId == toUserId || amount <= 0)
                return false;

            var sender = await _context.Users.FindAsync(fromUserId);
            var recipient = await _context.Users.FindAsync(toUserId);

            if (sender == null || recipient == null || sender.Coins < amount)
                return false;

            sender.UpdateCoins(sender.Coins - amount);
            recipient.UpdateCoins(recipient.Coins + amount);

            var transaction = new Transaction(fromUserId, toUserId, amount, DateTime.UtcNow);
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}

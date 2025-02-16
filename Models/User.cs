namespace AvitoTestTask.Models
{
    public class User
    {
        public int Id { get; private set; }
        public string Username { get; private set; }
        public string PasswordHash { get; private set; }
        public int Coins { get; private set; } = 1000;
        public List<MerchItem> Inventory { get; private set; } = new List<MerchItem>();

        public User(string username, string passwordHash)
        {
            Username = username;
            PasswordHash = passwordHash;
        }

        public void AddToInventory(MerchItem item)
        {
            Inventory.Add(item);
        }

        public void RemoveFromInventory(MerchItem item)
        {
            Inventory.Remove(item);
        }

        public void UpdateCoins(int amount)
        {
            if (amount < 0) throw new InvalidOperationException("Невозможно установить отрицательное количество монет.");
            Coins = amount;
        }

    }

}

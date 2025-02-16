namespace AvitoTestTask.Models
{
    public class Transaction
    {
        public int Id { get; private set; }
        public int FromUserId { get; private set; }
        public int ToUserId { get; private set; }
        public int Amount { get; private set; }
        public DateTime Timestamp { get; private set; }

        public Transaction(int fromUserId, int toUserId, int amount, DateTime timestamp)
        {
            FromUserId = fromUserId;
            ToUserId = toUserId;
            Amount = amount;
            Timestamp = timestamp;
        }
    }
}

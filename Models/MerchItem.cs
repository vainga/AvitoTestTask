namespace AvitoTestTask.Models
{
    public class MerchItem
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int Price { get; private set; }

        public MerchItem(string name, int price)
        {
            Name = name;
            Price = price;
        }
    }

}

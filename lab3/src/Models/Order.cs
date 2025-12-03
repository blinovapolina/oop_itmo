namespace DeliveryOrderManagementSystem.Models
{
    public class Order
    {
        public int Id { get; private set; }
        public Customer Customer { get; private set; }
        public List<OrderItem> Items { get; private set; }
        public DateTime OrderTime { get; private set; }
        public string Status { get; private set; }
        public string DeliveryAddress { get; private set; }
        public bool IsFastDelivery { get; private set; }
        public string SpecialPreferences { get; private set; }

        public Order(Customer customer, List<OrderItem> items, string deliveryAddress,
                    bool isFastDelivery = false, string specialPreferences = "", string status = "Preparing")
        {
            Customer = customer;
            Items = items ?? new List<OrderItem>();
            DeliveryAddress = deliveryAddress;
            IsFastDelivery = isFastDelivery;
            SpecialPreferences = specialPreferences;
            OrderTime = DateTime.Now;
            Status = status;
        }

        public decimal CalculateSumTotal()
        {
            return Items.Sum(item => item.GetTotalPrice());
        }
    }
}

namespace DeliverySystem.Models
{
    public class OrderItem
    {
        public MenuItem MenuItem { get; private set; }
        public int Quantity { get; private set; }
        public string SpecialInstructions { get; private set; }

        public OrderItem(MenuItem menuItem, int quantity, string specialInstructions = "")
        {
            MenuItem = menuItem;
            Quantity = quantity;
            SpecialInstructions = specialInstructions;
        }

        public decimal GetTotalPrice()
        {
            return MenuItem.Price * Quantity;
        }
    }
}
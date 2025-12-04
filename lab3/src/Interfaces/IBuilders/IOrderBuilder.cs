using DeliverySystem.Models;

namespace DeliverySystem.Builders.Interfaces
{
    public interface IOrderBuilder : IBuilder<Order>
    {
        IOrderBuilder SetCustomer(Customer customer);
        IOrderBuilder SetDeliveryAddress(string address);
        IOrderBuilder SetFastDelivery(bool isFastDelivery = true);
        IOrderBuilder SetSpecialPreferences(string preferences);

        IOrderBuilder AddItem(OrderItem item);
        IOrderBuilder AddDish(Dish dish, int quantity, string instructions = "");


        bool Validate(out List<string> errors);
    }
}
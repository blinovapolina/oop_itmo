using DeliverySystem.Models;
using DeliverySystem.Interfaces;

namespace DeliverySystem.Builders.Interfaces
{
    public interface IOrderBuilder : IBuilder<Order>
    {
        IOrderBuilder SetCustomer(Customer customer);
        IOrderBuilder SetDeliveryAddress(string address);
        IOrderBuilder SetFastDelivery(bool isFastDelivery = true);
        IOrderBuilder SetSpecialPreferences(string preferences);
        IOrderBuilder SetMediator(IOrderMediator mediator);

        IOrderBuilder AddItem(OrderItem item);
        IOrderBuilder AddDish(Dish dish, int quantity, string instructions = "");


        bool Validate();
    }
}
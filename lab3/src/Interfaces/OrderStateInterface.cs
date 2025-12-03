using DeliverySystem.Models;

namespace DeliverySystem.Interfaces
{
    public interface IOrderState
    {
        void ProcessOrder(Order order);
        string GetStatus();
    }
}
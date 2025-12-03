using DeliverySystem.Models;

namespace DeliverySystem.Interfaces
{
    public interface IOrderServiceComponent
    {
        void HandleOrderCreated(Order order);
        void HandleOrderStatusChanged(Order order, string oldStatus, string newStatus);
        string GetServiceName();
    }
}
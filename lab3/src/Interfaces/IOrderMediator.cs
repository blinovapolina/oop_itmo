using DeliverySystem.Models;

namespace DeliverySystem.Interfaces
{
    public interface IOrderMediator
    {
        void NotifyOrderCreated(Order order);
        void NotifyOrderStatusChanged(Order order, string oldStatus, string newStatus);
        void RegisterService(string serviceName, IOrderServiceComponent component);
    }
}
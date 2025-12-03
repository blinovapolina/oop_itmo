using DeliverySystem.Interfaces;
using DeliverySystem.Models;

namespace DeliverySystem.States
{
    public abstract class OrderState : IOrderState
    {
        public abstract void ProcessOrder(Order order);
        public abstract string GetStatus();
    }
}
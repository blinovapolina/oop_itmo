using DeliverySystem.Models;

namespace DeliverySystem.States
{
    public class PendingState : OrderState
    {
        public override void ProcessOrder(Order order)
        {
            Console.WriteLine($"Заказ с номером {order.Id} ожидает подтверждения...");
        }

        public override string GetStatus()
        {
            return "Ожидает подтверждения";
        }
    }
}
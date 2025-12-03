using DeliverySystem.Models;

namespace DeliverySystem.States
{
    public class InDeliveryState : OrderState
    {
        public override void ProcessOrder(Order order)
        {
            Console.WriteLine($"Заказ с номером {order.Id} сейчас доставляется...");
        }

        public override string GetStatus()
        {
            return "В доставке";
        }
    }
}
using DeliverySystem.Models;

namespace DeliverySystem.States
{
    public class ReadyForDeliveryState : OrderState
    {
        public override void ProcessOrder(Order order)
        {
            Console.WriteLine($"Заказ с номером {order.Id} готов к отправке курьеру");
        }

        public override string GetStatus()
        {
            return "Готов к доставке";
        }
    }
}
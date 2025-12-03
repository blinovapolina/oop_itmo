using DeliverySystem.Models;

namespace DeliverySystem.States
{
    public class CancelledState : OrderState
    {
        public override void ProcessOrder(Order order)
        {
            Console.WriteLine($"Заказ с номером {order.Id} отменен!");
        }

        public override string GetStatus()
        {
            return "Отменен";
        }
    }
}
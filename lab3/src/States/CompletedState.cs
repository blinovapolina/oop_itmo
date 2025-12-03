using DeliverySystem.Models;

namespace DeliverySystem.States
{
    public class CompletedState : OrderState
    {
        public override void ProcessOrder(Order order)
        {
            Console.WriteLine($"Заказ с номером {order.Id} выполнен!");
        }

        public override string GetStatus()
        {
            return "Выполнен";
        }
    }
}
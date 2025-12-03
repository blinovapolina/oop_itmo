using DeliverySystem.Models;

namespace DeliverySystem.States
{
    public class PreparingState : OrderState
    {
        public override void ProcessOrder(Order order)
        {
            Console.WriteLine($"Закак с номером {order.Id} готовится...");
        }

        public override string GetStatus()
        {
            return "Готовится";
        }
    }
}
using DeliverySystem.Models;

namespace DeliverySystem.States
{
    public class CourierAssignedState : OrderState
    {
        public override void ProcessOrder(Order order)
        {
            Console.WriteLine($"Курьер назначен для заказа номер {order.Id}");
        }

        public override string GetStatus()
        {
            return "Курьер назначен";
        }
    }
}
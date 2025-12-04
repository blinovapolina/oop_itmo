using DeliveryOrderManagementSystem.Interfaces;
using DeliveryOrderManagementSystem.Models;

namespace DeliveryOrderManagementSystem.Services
{
    public class KitchenService : IOrderServiceComponent
    {
        private IOrderMediator Mediator { get; }

        public KitchenService(IOrderMediator mediator)
        {
            Mediator = mediator;
            mediator.RegisterService("KitchenService", this);
        }

        public string GetServiceName() => "KitchenService";

        public void HandleOrderCreated(Order order)
        {
            Console.WriteLine($"Кухня: Начало приготовления заказа номер {order.Id}");

            var preparationTime = order.Items.Sum(i => i.MenuItem.PreparationTime);
            Console.WriteLine($"Кухня: ** Прошло {preparationTime} времени **");

            Console.WriteLine($"Кухня: Заказ номер {order.Id} готов");
        }

        public void HandleOrderStatusChanged(Order order, string oldStatus, string newStatus)
        {
            if (newStatus == "В доставке")
            {
                Console.WriteLine($"Кухня: Заказ номер {order.Id} передан на доставку");
            }
        }
    }
}
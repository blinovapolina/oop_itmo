using DeliveryOrderManagementSystem.Interfaces;
using DeliveryOrderManagementSystem.Models;

namespace DeliveryOrderManagementSystem.Services
{
    public class DeliveryService : IOrderServiceComponent
    {
        private IOrderMediator Mediator { get; }

        public DeliveryService(IOrderMediator mediator)
        {
            Mediator = mediator;
            mediator.RegisterService("DeliveryService", this);
        }

        public string GetServiceName() => "DeliveryService";

        public void HandleOrderCreated(Order order)
        {
            Console.WriteLine($"Служба доставки: Поиск курьера для заказа номер {order.Id}");

            if (order.Customer.HasPriorityDelivery())
            {
                Console.WriteLine($"Служба доставки: Приоритетный курьер назначен для VIP заказа номер {order.Id}");
            }
            else
            {
                Console.WriteLine($"Служба доставки: ** Прошло n-ое кол-во времени **");
                Console.WriteLine($"Служба доставки: Курьер найден для заказа номер {order.Id}");
            }
        }

        public void HandleOrderStatusChanged(Order order, string oldStatus, string newStatus)
        {
            if (newStatus == "В доставке")
            {
                Console.WriteLine($"Служба доставки: Курьер забрал заказ номер {order.Id}");

                Console.WriteLine($"Служба доставки: ** Прошло n-ое кол-во времени **");
                Console.WriteLine($"Служба доставки: Заказ номер {order.Id} доставлен");
            }
        }
    }
}
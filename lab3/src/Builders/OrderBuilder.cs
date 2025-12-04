using DeliverySystem.Models;
using DeliverySystem.States;
using DeliverySystem.Builders.Interfaces;

namespace DeliverySystem.Builders
{
    public class OrderBuilder : IOrderBuilder
    {
        private Customer _customer;
        private string _deliveryAddress;
        private bool _isFastDelivery = false;
        private string _specialPreferences = "";
        private readonly List<OrderItem> _items = new();

        public IOrderBuilder SetCustomer(Customer customer)
        {
            _customer = customer ?? throw new ArgumentNullException(nameof(customer), "Клиент не может быть null");
            return this;
        }

        public IOrderBuilder SetDeliveryAddress(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("Адрес доставки не может быть пустым");

            address = address.Trim();

            _deliveryAddress = address;
            return this;
        }

        public IOrderBuilder SetFastDelivery(bool isFastDelivery = true)
        {
            _isFastDelivery = isFastDelivery;

            if (isFastDelivery && _items.Count > 5)
            {
                Console.WriteLine("Внимание: Быстрая доставка для большого заказа может быть ограничена");
            }

            return this;
        }

        public IOrderBuilder SetSpecialPreferences(string preferences)
        {
            _specialPreferences = preferences ?? "";
            return this;
        }

        public IOrderBuilder AddItem(OrderItem item)
        {
            _items.Add(item ?? throw new ArgumentNullException(nameof(item), "Элемент заказа не может быть null"));
            return this;
        }

        public IOrderBuilder AddDish(Dish dish, int quantity, string instructions = "")
        {
            if (dish == null)
                throw new ArgumentNullException(nameof(dish), "Блюдо не может быть null");
            if (quantity <= 0)
                throw new ArgumentException("Количество должно быть положительным числом", nameof(quantity));

            _items.Add(new OrderItem(dish, quantity, instructions));
            return this;
        }

        public bool Validate(out List<string> errors)
        {
            errors = new List<string>();

            if (_customer == null)
                errors.Add("Не указан клиент");

            if (string.IsNullOrWhiteSpace(_deliveryAddress))
                errors.Add("Не указан адрес доставки");

            if (_items.Count == 0)
                errors.Add("Заказ не содержит блюд");


            return errors.Count == 0;
        }


        public int CalculateEstimatedPreparationTime()
        {
            if (_items.Count == 0)
                return 0;

            return _items.Max(i => i.Dish.PreparationTime) + 10; // +10 минут на упаковку
        }

        public Order Build()
        {
            List<string> errors;
            if (!Validate(out errors))
                throw new InvalidOperationException($"Невозможно создать заказ:\n{string.Join("\n", errors)}");

            var order = new Order(_customer, _items, _deliveryAddress, _isFastDelivery, _specialPreferences);

            if (_autoApprove && order.GetStatus() == "Ожидает подтверждения")
            {
                try
                {
                    order.Approve();
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine($"Не удалось автоматически подтвердить заказ: {ex.Message}");
                }
            }

            return order;
        }

        public bool CanBuild()
        {
            return Validate(out _);
        }
    }
}
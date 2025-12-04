using DeliverySystem.Builders;
using DeliverySystem.Builders.Interfaces;
using DeliverySystem.Enums;
using DeliverySystem.Interfaces;
using DeliverySystem.Mediators;
using DeliverySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeliverySystem.Core
{
    public class DeliverySystemDemo
    {
        private DeliverySystemApp _system;
        private DataLoader _dataLoader;
        private IOrderMediator _mediator;
        private ICustomerBuilder _customerBuilder;
        private IDishBuilder _dishBuilder;

        public DeliverySystemDemo()
        {
            // Создаём билдеры один раз
            _customerBuilder = new CustomerBuilder();
            _dishBuilder = new DishBuilder();

            // Передаём их в DataLoader
            _dataLoader = new DataLoader(_customerBuilder, _dishBuilder);
        }

        public void Run()
        {
            Console.WriteLine("=== СИСТЕМА ДОСТАВКИ ЕДЫ ===\n");

            InitializeSystem();
            LoadData();
            DemonstrateSystem();
            SaveData();
        }

        private void InitializeSystem()
        {
            _mediator = new OrderProcessingMediator();

            // Используем уже созданные билдеры
            _system = new DeliverySystemApp(_mediator, _customerBuilder, _dishBuilder);
        }

        private void LoadData()
        {
            _dataLoader.EnsureDataDirectory();

            var customers = _dataLoader.LoadCustomers();
            foreach (var customer in customers)
            {
                _system.Customers.Add(customer);
            }

            var menu = _dataLoader.LoadMenu();
            foreach (var dish in menu)
            {
                _system.Menu.Add(dish);
            }
        }

        private void DemonstrateSystem()
        {
            Console.WriteLine("\n=== ДЕМОНСТРАЦИЯ РАБОТЫ СИСТЕМЫ ===\n");

            if (_system.Customers.Count == 0)
            {
                throw new InvalidOperationException("Нет клиентов для демонстрации");
            }

            if (_system.Menu.Count == 0)
            {
                throw new InvalidOperationException("Нет блюд в меню для демонстрации");
            }

            var firstCustomer = _system.Customers.First();
            var pizza = _system.Menu.First(d => d.Name.Contains("Пепперони"));
            var cola = _system.Menu.First(d => d.Name.Contains("Кола"));

            // 1. Создание заказов
            Console.WriteLine("1) СОЗДАНИЕ И ОБРАБОТКА ЗАКАЗОВ");

            // Заказ 1: Используем Builder напрямую
            var order1 = new OrderBuilder()
                .SetCustomer(firstCustomer)
                .SetDeliveryAddress(firstCustomer.Address)
                .SetFastDelivery(false)
                .SetSpecialPreferences("Без перца")
                .SetMediator(_mediator)
                .AddDish(pizza, 1, "Без перца")
                .Build();
            _system.Orders.Add(order1);

            // Заказ 2: Используем Builder с разными блюдами
            var order2 = new OrderBuilder()
                .SetCustomer(firstCustomer)
                .SetDeliveryAddress(firstCustomer.Address)
                .SetFastDelivery(true)
                .SetMediator(_mediator)
                .AddDish(pizza, 2, "Острая")
                .AddDish(cola, 3)
                .Build();
            _system.Orders.Add(order2);

            // Обработка заказа 1
            _system.ApproveOrder(order1.Id);
            _system.CompletePreparation(order1.Id);
            _system.AssignCourier(order1.Id);
            _system.StartDelivery(order1.Id);
            _system.CompleteDelivery(order1.Id);

            Console.WriteLine("\n2) ОТМЕНА ЗАКАЗА");

            // Заказ 3: Используем Builder для отмены
            var order3 = new OrderBuilder()
                .SetCustomer(firstCustomer)
                .SetDeliveryAddress(firstCustomer.Address)
                .SetMediator(_mediator)
                .AddDish(pizza, 1)
                .Build();
            _system.Orders.Add(order3);
            _system.CancelOrder(order3.Id, "Передумал клиент");

            Console.WriteLine("\n3) РАСЧЕТ СТОИМОСТИ");

            var total1 = _system.CalculateOrderTotal(order1.Id);
            var total2 = _system.CalculateOrderTotal(order2.Id);

            Console.WriteLine($"Стоимость заказа 1: {total1:F2} рублей");
            Console.WriteLine($"Стоимость заказа 2: {total2:F2} рублей");

            Console.WriteLine("\n4) ПРОВЕРКА СКИДОК И БЕСПЛАТНОЙ ДОСТАВКИ");

            foreach (var customer in _system.Customers)
            {
                var discount = _system.GetCustomerDiscountPercentage(customer.Id) * 100;
                var freeDelivery = _system.CanHaveFreeDelivery(customer.Id);

                Console.WriteLine($"Клиент {customer.Name}: скидка {discount:F2}%, " +
                     $"бесплатная доставка: {(freeDelivery ? "Да" : "Нет")}");
            }
        }

        private void SaveData()
        {
            _dataLoader.SaveCustomers(_system.Customers);
            _dataLoader.SaveMenu(_system.Menu);
            _dataLoader.SaveOrdersSummary(_system.Orders);

            Console.WriteLine("\n=== ДЕМОНСТРАЦИЯ ЗАВЕРШЕНА ===\n");
            Console.WriteLine("Данные сохранены в папке 'Data'.");
        }
    }
}
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

        public DeliverySystemDemo()
        {
            _dataLoader = new DataLoader();
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
            var mediator = new OrderProcessingMediator();
            var customerBuilder = new CustomerBuilder();
            var dishBuilder = new DishBuilder();

            _system = new DeliverySystemApp(mediator, customerBuilder, dishBuilder);
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

            var order1 = _system.CreateOrder(firstCustomer.Id, firstCustomer.Address,
                isFastDelivery: false, specialPreferences: "Без перца");

            var items = new List<OrderItem>
            {
                new OrderItem(pizza, 2, "Острая"),
                new OrderItem(cola, 3)
            };

            var order2 = _system.CreateOrderWithItems(firstCustomer.Id, firstCustomer.Address,
                items, isFastDelivery: true);

            _system.ApproveOrder(order1.Id);
            _system.CompletePreparation(order1.Id);
            _system.AssignCourier(order1.Id);
            _system.StartDelivery(order1.Id);
            _system.CompleteDelivery(order1.Id);

            Console.WriteLine("\n2) ОТМЕНА ЗАКАЗА");

            var order3 = _system.CreateOrder(firstCustomer.Id, firstCustomer.Address);
            _system.CancelOrder(order3.Id, "Передумал клиент");

            Console.WriteLine("\n3) РАСЧЕТ СТОИМОСТИ");

            var total1 = _system.CalculateOrderTotal(order1.Id);
            var total2 = _system.CalculateOrderTotal(order2.Id);

            Console.WriteLine("\n4) ПРОВЕРКА СКИДОК И БЕСПЛАТНОЙ ДОСТАВКИ");

            foreach (var customer in _system.Customers)
            {
                var discount = _system.GetCustomerDiscountPercentage(customer.Id) * 100;
                var freeDelivery = _system.CanHaveFreeDelivery(customer.Id);
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
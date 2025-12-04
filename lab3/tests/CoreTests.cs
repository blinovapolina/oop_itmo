using Xunit;
using Moq;
using DeliverySystem.Core;
using DeliverySystem.Models;
using DeliverySystem.Enums;
using DeliverySystem.Interfaces;
using DeliverySystem.Mediators;

namespace DeliverySystem.Tests
{
    public class CoreTests
    {
        private readonly DeliverySystemApp _system;

        public CoreTests()
        {
            var mediatorMock = new Mock<IOrderMediator>();
            _system = new DeliverySystemApp(
                mediatorMock.Object,
                new DeliverySystem.Builders.CustomerBuilder(),
                new DeliverySystem.Builders.DishBuilder());
        }

        [Fact]
        public void CreateCustomer_AddsCustomerToList()
        {
            // Act - ИСПРАВЬТЕ ТЕЛЕФОН
            var customer = _system.CreateCustomer(
                "Иван Иванов",
                "+79123456789", // Минимум 10 цифр
                "ул. Ленина, 10");

            // Assert
            Assert.Single(_system.Customers);
            Assert.Equal("Иван Иванов", customer.Name);
        }

        [Fact]
        public void CreateOrder_WithValidData_CreatesOrder()
        {
            // Arrange - ИСПРАВЬТЕ ВСЕ ТЕЛЕФОНЫ
            var customer = _system.CreateCustomer(
                "Иван",
                "+79123456789", // 10 цифр
                "ул. Ленина, 10");

            var dish = _system.CreateDish("Пицца", "", 500, 15);
            var item = new OrderItem(dish, 2);

            // Act
            var order = _system.CreateOrderWithItems(
                customer.Id,
                "ул. Пушкина, 20",
                new List<OrderItem> { item });

            // Assert
            Assert.Single(_system.Orders);
            Assert.Equal(customer, order.Customer);
        }

        [Fact]
        public void ApproveOrder_ChangesStatusToPreparing()
        {
            // Arrange - ИСПРАВЬТЕ ТЕЛЕФОН
            var customer = _system.CreateCustomer(
                "Иван",
                "+79123456789", // 10 цифр
                "ул. Ленина, 10");

            var dish = _system.CreateDish("Пицца", "", 500, 15);
            var order = _system.CreateOrderWithItems(
                customer.Id,
                "ул. Пушкина, 20",
                new List<OrderItem> { new(dish, 1) });

            // Act
            _system.ApproveOrder(order.Id);

            // Assert
            Assert.Equal("Готовится", _system.GetOrderStatus(order.Id));
        }

        [Fact]
        public void CalculateOrderTotal_IncludesTaxAndDelivery()
        {
            // Arrange - ИСПРАВЬТЕ ТЕЛЕФОН
            var customer = _system.CreateCustomer(
                "Иван",
                "+79123456789", // 10 цифр
                "ул. Ленина, 10");

            var dish = _system.CreateDish("Пицца", "", 1000, 15);
            var order = _system.CreateOrderWithItems(
                customer.Id,
                "ул. Пушкина, 20",
                new List<OrderItem> { new(dish, 2) });

            // Act
            var total = _system.CalculateOrderTotal(order.Id);

            // Assert
            var expectedSubtotal = 2000m; // 1000 * 2
            var tax = expectedSubtotal * 0.1m; // 10% налог
            var delivery = 100m; // Стандартная доставка
            var expectedTotal = expectedSubtotal + tax + delivery; // 2000 + 200 + 100 = 2300

            Assert.Equal(expectedTotal, total);
        }

        [Fact]
        public void GetCustomerDiscount_VIP_Returns10Percent()
        {
            // Arrange - ИСПРАВЬТЕ ТЕЛЕФОН
            var customer = _system.CreateCustomer(
                "VIP Клиент",
                "+79123456789", // 10 цифр
                "ул. Ленина, 10",
                CustomerCategory.VIP);

            // Act
            var discount = _system.GetCustomerDiscountPercentage(customer.Id);

            // Assert
            Assert.Equal(0.10m, discount);
        }
    }
}
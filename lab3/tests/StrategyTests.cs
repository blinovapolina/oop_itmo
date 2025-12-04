using Xunit;
using Moq;
using DeliverySystem.Strategies;
using DeliverySystem.Models;
using DeliverySystem.Enums;
using DeliverySystem.Interfaces;
using DeliverySystem.Mediators;

namespace DeliverySystem.Tests
{
    public class StrategyTests
    {
        [Fact]
        public void StandardDeliveryStrategy_RegularCustomerSmallOrder_AddsDeliveryFee()
        {
            // Arrange
            var strategy = new StandardDeliveryStrategy();
            var customer = new Customer(1, "Иван", "+7912", "ул. Ленина, 10", CustomerCategory.Regular);
            var dish = new Dish(1, "Бургер", "", 300, 10);
            var items = new List<OrderItem> { new(dish, 1) };
            var order = new Order(1, customer, items, "ул. Ленина, 10", new Mock<IOrderMediator>().Object);

            // Act
            var total = strategy.CalculateTotal(order);

            // Assert
            var subtotal = 300m;
            var tax = subtotal * 0.1m; // 30
            var delivery = 100m; // Стандартная доставка
            var expected = subtotal + tax + delivery; // 300 + 30 + 100 = 430

            Assert.Equal(expected, total);
        }

        [Fact]
        public void StandardDeliveryStrategy_VIPCustomer_FreeDelivery()
        {
            // Arrange
            var strategy = new StandardDeliveryStrategy();
            var customer = new Customer(1, "VIP", "+7912", "ул. Ленина, 10", CustomerCategory.VIP);
            var dish = new Dish(1, "Стейк", "", 2000, 30);
            var items = new List<OrderItem> { new(dish, 1) };
            var order = new Order(1, customer, items, "ул. Ленина, 10", new Mock<IOrderMediator>().Object);

            // Act
            var total = strategy.CalculateTotal(order);

            // Assert
            var subtotal = 2000m;
            var tax = subtotal * 0.1m; // 200
            var discount = subtotal * 0.10m; // 200 (VIP скидка 10%)
            var expected = subtotal + tax - discount; // 2000 + 200 - 200 = 2000

            Assert.Equal(expected, total);
        }

        [Fact]
        public void ExpressDeliveryStrategy_AddsExpressCharge()
        {
            // Arrange
            var strategy = new ExpressDeliveryStrategy();
            var customer = new Customer(1, "Иван", "+7912", "ул. Ленина, 10", CustomerCategory.Regular);
            var dish = new Dish(1, "Пицца", "", 1000, 15);
            var items = new List<OrderItem> { new(dish, 1) };
            var order = new Order(1, customer, items, "ул. Ленина, 10", new Mock<IOrderMediator>().Object, true);

            // Act
            var total = strategy.CalculateTotal(order);

            // Assert
            var subtotal = 1000m;
            var tax = subtotal * 0.1m; // 100
            var delivery = 500m; // Экспресс доставка
            var serviceCharge = subtotal * 0.02m; // 20 (2% сбор)
            var expected = subtotal + tax + delivery + serviceCharge; // 1000 + 100 + 500 + 20 = 1620

            Assert.Equal(expected, total);
        }

        [Fact]
        public void ExpressDeliveryStrategy_SuperVIP_LargeOrder_FreeExpressDelivery()
        {
            // Arrange
            var strategy = new ExpressDeliveryStrategy();
            var customer = new Customer(1, "SuperVIP", "+7912", "ул. Ленина, 10", CustomerCategory.SuperVIP);
            var dish = new Dish(1, "Дорогое блюдо", "", 6000, 45);
            var items = new List<OrderItem> { new(dish, 1) };
            var order = new Order(1, customer, items, "ул. Ленина, 10", new Mock<IOrderMediator>().Object, true);

            // Act
            var total = strategy.CalculateTotal(order);

            // Assert
            var subtotal = 6000m;
            var tax = subtotal * 0.1m; // 600
            var discount = subtotal * 0.15m; // 900 (SuperVIP скидка 15%)
            var serviceCharge = subtotal * 0.02m; // 120 (2% сбор)
            var expected = subtotal + tax + serviceCharge - discount; // 6000 + 600 + 120 - 900 = 5820

            Assert.Equal(expected, total);
        }
    }
}
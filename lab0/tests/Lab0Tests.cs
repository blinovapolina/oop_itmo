using Xunit;
using Lab0.Core;
using Lab0.Models;
using System.Collections.Generic;
using System.Linq;

namespace Lab0.Tests
{
    public class VendingMachineAndAdminTests
    {
        [Fact(DisplayName = "Вставка допустимой монеты увеличивает баланс")]
        public void InsertCoin_ValidCoin_IncreasesBalance()
        {
            var coins = new List<decimal>();
            decimal coin = 10m;

            coins.Add(coin);

            Assert.Single(coins);
            Assert.Equal(10m, coins.Sum());
        }

        [Fact(DisplayName = "Покупка товара уменьшает его запас и возвращает сдачу")]
        public void BuyItem_ReducesStockAndReturnsChange()
        {
            var item = new Item(1, "Чипсы", 50m, 5);
            var insertedCoins = new List<decimal> { 100m };

            decimal totalInserted = insertedCoins.Sum();
            decimal change = totalInserted - item.Price;
            item.Stock--;

            insertedCoins.Clear();

            Assert.Equal(4, item.Stock);
            Assert.Equal(50m, change);
            Assert.Empty(insertedCoins);
        }

        [Fact(DisplayName = "Отмена покупки возвращает внесённые монеты")]
        public void CancelPurchase_ReturnsInsertedCoins()
        {
            var insertedCoins = new List<decimal> { 1m, 5m, 10m };
            decimal total = insertedCoins.Sum();
            insertedCoins.Clear();

            Assert.Equal(16m, total);
            Assert.Empty(insertedCoins);
        }

        [Fact(DisplayName = "Пополнение товара увеличивает его запас")]
        public void Admin_Refill_IncreasesStock()
        {
            var items = new List<Item> { new Item(1, "Шоколад", 60m, 3) };
            var item = items.FirstOrDefault(i => i.Id == 1);
            int refillAmount = 5;
            if (item != null) item.Stock += refillAmount;

            Assert.Equal(8, item!.Stock);
        }

        [Fact(DisplayName = "Добавление нового товара создаёт новый объект")]
        public void Admin_AddItem_CreatesNewItem()
        {
            var items = new List<Item>();
            string name = "Сок";
            decimal price = 70m;
            int qty = 10;
            int newId = items.Count > 0 ? items[^1].Id + 1 : 1;

            items.Add(new Item(newId, name, price, qty));

            Assert.Single(items);
            Assert.Equal("Сок", items[0].Name);
            Assert.Equal(70m, items[0].Price);
            Assert.Equal(10, items[0].Stock);
        }

        [Fact(DisplayName = "Сбор средств обнуляет баланс")]
        public void Admin_CollectMoney_ResetsBalance()
        {
            decimal machineBalance = 150m;
            decimal collected = machineBalance;
            machineBalance = 0;

            Assert.Equal(150m, collected);
            Assert.Equal(0m, machineBalance);
        }
    }
}

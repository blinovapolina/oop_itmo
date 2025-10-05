using Lab1.Models;
using DotNetEnv;

namespace Lab1.Core
{
    public class VendingMachine
    {
        private const string ItemsPathJson = "Data/items.json";

        private List<Item> Items;
        private decimal MachineBalance = 0m;
        private List<decimal> InsertedCoins = [];
        private List<decimal> AcceptedNominalValue = [1m, 2m, 5m, 10m, 20m, 50m, 100m];

        public VendingMachine()
        {
            Env.Load();
            Items = Utils.LoadItemsJson(ItemsPathJson);
        }

        public void Run()
        {
            while (true)
            {
                Console.Clear();
                ShowMainMenu();
                ConsoleKeyInfo key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.D1: ShowItems(); break;
                    case ConsoleKey.D2: InsertCoin(); break;
                    case ConsoleKey.D3: BuyItem(); break;
                    case ConsoleKey.D4: CancelPurchase(); break;
                    case ConsoleKey.D5:
                        new AdminPanel(Items, ItemsPathJson, MachineBalance).Open();
                        break;
                    case ConsoleKey.D0:
                        Console.WriteLine("\nДо свидания!\nВыход из программы...");
                        return;

                    default:
                        Console.WriteLine("Неверный выбор. Нажмите любую клавишу >>>");
                        Console.ReadKey(true);
                        break;
                }
            }
        }

        private void ShowMainMenu()
        {
            Console.WriteLine("----- Вендинговый автомат: Главное меню -----");
            Console.WriteLine("1) Показать товары — просмотреть список и остаток");
            Console.WriteLine("2) Вставить монету — добавить деньги");
            Console.WriteLine("3) Купить товар — выбрать ID товара для покупки");
            Console.WriteLine("4) Отмена и возврат монет — вернуть внесённые средства");
            Console.WriteLine("5) Админ-панель — управление автоматом");
            Console.WriteLine("0) Выход — завершение программы");
            Console.WriteLine($"\nТекущий баланс: {InsertedCoins.Sum():0.00} ₽");
            Console.WriteLine("Выберите пункт, нажав цифру >>>");
        }

        private void ShowItems()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("----- Вендинговый автомат: Показать товары -----");
                Console.WriteLine("ID | Название     | Цена    | Остаток");
                Console.WriteLine("---|--------------|---------|--------");
                foreach (var item in Items)
                    Console.WriteLine($"{item.Id,2} | {item.Name,-12} | {item.Price,7:0.00} | {item.Stock,3}");

                Console.WriteLine($"\nТекущий баланс: {InsertedCoins.Sum():0.00} ₽");
                Console.WriteLine("Подсказка: для возврата в главное меню нажмите 0.");
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.D0) break;
            }
        }

        private void InsertCoin()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("----- Вендинговый автомат: Вставить монету -----");
                Console.WriteLine("Доступные монеты: " + string.Join(", ", AcceptedNominalValue));
                Console.WriteLine($"Текущий баланс: {InsertedCoins.Sum():0.00} ₽");
                Console.WriteLine("Введите номинал монеты или 0 для возврата в меню:");

                string? input = Console.ReadLine();
                if (input == "0") break;

                if (decimal.TryParse(input, out decimal coin) && AcceptedNominalValue.Contains(coin))
                {
                    InsertedCoins.Add(coin);
                    Console.WriteLine($"Внесено {coin:0.00} ₽ (Баланс: {InsertedCoins.Sum():0.00} ₽)");
                }
                else
                    Console.WriteLine("Неверный номинал. Попробуйте снова.");

                Console.WriteLine("Нажмите любую клавишу для продолжения >>>");
                Console.ReadKey(true);
            }
        }

        private void BuyItem()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("----- Вендинговый автомат: Купить товар -----");
                Console.WriteLine("ID | Название     | Цена    | Остаток");
                Console.WriteLine("---|--------------|---------|--------");
                foreach (var item in Items)
                    Console.WriteLine($"{item.Id,2} | {item.Name,-12} | {item.Price,7:0.00} | {item.Stock,3}");

                Console.WriteLine($"\nТекущий баланс: {InsertedCoins.Sum():0.00} ₽");
                Console.WriteLine("Введите ID товара для покупки или 0 для выхода в меню:");

                string? input = Console.ReadLine();
                if (input == "0") break;

                if (!int.TryParse(input, out int id))
                {
                    Console.WriteLine("Неверный ввод. Попробуйте снова.");
                    Console.ReadKey(true);
                    continue;
                }

                Item? itemBuy = null;
                foreach (var item in Items)
                {
                    if (item.Id == id)
                    {
                        itemBuy = item;
                        break;
                    }
                }

                if (itemBuy == null)
                {
                    Console.WriteLine("Товар не найден.");
                    Console.ReadKey(true);
                    continue;
                }

                if (InsertedCoins.Sum() < itemBuy.Price)
                {
                    Console.WriteLine($"Недостаточно средств. Для покупки необходимо ещё {itemBuy.Price - InsertedCoins.Sum():0.00} ₽");
                    Console.ReadKey(true);
                    continue;
                }

                if (itemBuy.Stock <= 0)
                {
                    Console.WriteLine("Товар закончился.");
                    Console.ReadKey(true);
                    continue;
                }

                itemBuy.Stock--;
                MachineBalance += itemBuy.Price;
                decimal change = InsertedCoins.Sum() - itemBuy.Price;
                InsertedCoins.Clear();

                Console.WriteLine($"Вы купили {itemBuy.Name} за {itemBuy.Price:0.00} ₽");
                if (change > 0)
                    Console.WriteLine($"Ваша сдача: {change:0.00} ₽");
                Console.WriteLine($"Баланс после покупки: {InsertedCoins.Sum():0.00} ₽");

                Utils.SaveItemsJson(ItemsPathJson, Items);
                Console.WriteLine("Нажмите любую клавишу для возврата в главное меню >>>");
                Console.ReadKey(true);
                break;
            }
        }

        private void CancelPurchase()
        {
            Console.Clear();
            Console.WriteLine("----- Вендинговый автомат: Отмена и возврат монет -----");

            decimal returnedMoney = InsertedCoins.Sum();
            if (returnedMoney == 0)
                Console.WriteLine("Вы ничего не внесли.");
            else
                Console.WriteLine($"Возвращено: {returnedMoney:0.00} ₽");

            InsertedCoins.Clear();
            Console.WriteLine($"Баланс после возврата: 0.00 ₽");
            Console.WriteLine("Нажмите любую клавишу для возврата в главное меню >>>");
            Console.ReadKey(true);
        }
    }
}

using Lab1.Models;
using DotNetEnv;

namespace Lab1.Core
{
    public class AdminPanel
    {
        private List<Item> Items;
        private string FilePathJson;
        private decimal MachineBalance;

        public AdminPanel(List<Item> items, string path, decimal revenue)
        {
            Items = items;
            FilePathJson = path;
            MachineBalance = revenue;

            Env.Load();
        }

        public void Open()
        {
            string adminPassword = Env.GetString("ADMIN_PASSWORD");

            while (true)
            {
                Console.Clear();
                Console.WriteLine("----- Админ-панель: Вход -----");
                Console.Write("Введите пароль администратора: ");
                string password = Utils.ReadPassword();

                if (password != adminPassword)
                {
                    Console.WriteLine("\nНеверный пароль. Попробуйте снова.");
                    Console.WriteLine("Нажмите любую клавишу >>>");
                    Console.ReadKey(true);
                    continue;
                }
                break;
            }

            while (true)
            {
                Console.Clear();
                Console.WriteLine("----- Админ-панель: Главное меню -----");
                Console.WriteLine("1) Показать товары");
                Console.WriteLine("2) Пополнить товар");
                Console.WriteLine("3) Добавить новый товар");
                Console.WriteLine("4) Сбор собранных средств");
                Console.WriteLine("5) Изменить пароль");
                Console.WriteLine("0) Вернуться в главное меню");
                Console.WriteLine("\nВыберите цифру:");

                ConsoleKey key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.D1: ShowItems(); break;
                    case ConsoleKey.D2: RefillItem(); break;
                    case ConsoleKey.D3: AddItem(); break;
                    case ConsoleKey.D4: CollectMoney(); break;
                    case ConsoleKey.D5: ChangePassword(); break;
                    case ConsoleKey.D0: return;
                    default:
                        Console.WriteLine("Неверный выбор. Нажмите любую клавишу >>>");
                        Console.ReadKey(true); break;
                }
            }
        }

        private void ShowItems()
        {
            Console.Clear();
            Console.WriteLine("----- Админ-панель: Показать товары -----");
            Console.WriteLine("ID | Название     | Цена    | Остаток");
            Console.WriteLine("---|--------------|---------|--------");
            foreach (var item in Items)
                Console.WriteLine($"{item.Id,2} | {item.Name,-12} | {item.Price,7:0.00} | {item.Stock,3}");

            Console.WriteLine("\nНажмите любую клавишу для возврата >>>");
            Console.ReadKey(true);
        }

        private void RefillItem()
        {
            Console.Clear();
            Console.WriteLine("----- Админ-панель: Пополнить товар -----");
            Console.Write("Введите ID товара: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) return;

            Item? item = null;

            foreach (var i in Items)
            {
                if (i.Id == id)
                {
                    item = i;
                    break;
                }
            }
            if (item == null) { Console.WriteLine("Товар не найден."); Console.ReadKey(true); return; }

            Console.Write("Введите количество для добавления: ");
            if (int.TryParse(Console.ReadLine(), out int amount) && amount > 0)
            {
                item.Stock += amount;
                Utils.SaveItemsJson(FilePathJson, Items);
                Console.WriteLine("Товар успешно пополнен!");
            }
            else Console.WriteLine("Некорректное количество.");

            Console.WriteLine("Нажмите любую клавишу для возврата >>>");
            Console.ReadKey(true);
        }

        private void AddItem()
        {
            Console.Clear();
            Console.WriteLine("----- Админ-панель: Добавить новый товар -----");

            Console.Write("Название: ");
            string? name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name)) { Console.WriteLine("Название не может быть пустым."); Console.ReadKey(true); return; }

            Console.Write("Цена: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price)) { Console.WriteLine("Некорректная цена."); Console.ReadKey(true); return; }

            Console.Write("Количество: ");
            if (!int.TryParse(Console.ReadLine(), out int qty)) { Console.WriteLine("Некорректное количество."); Console.ReadKey(true); return; }

            int newId = Items.Count > 0 ? Items[^1].Id + 1 : 1;
            Items.Add(new Item(newId, name, price, qty));
            Utils.SaveItemsJson(FilePathJson, Items);

            Console.WriteLine("Новый товар добавлен!");
            Console.WriteLine("Нажмите любую клавишу для возврата >>>");
            Console.ReadKey(true);
        }

        private void CollectMoney()
        {
            Console.Clear();
            Console.WriteLine("----- Админ-панель: Сбор собранных средств -----");
            Console.WriteLine($"Собранная сумма: {MachineBalance:0.00} ₽");
            MachineBalance = 0;
            Console.WriteLine("Средства успешно собраны!");
            Console.WriteLine("Нажмите любую клавишу для возврата >>>");
            Console.ReadKey(true);
        }

        private void ChangePassword()
        {
            Console.Clear();
            Console.WriteLine("----- Админ-панель: Изменить пароль -----");
            Console.Write("Введите новый пароль: ");
            string newPassword = Utils.ReadPassword();
            File.WriteAllText(".env", $"ADMIN_PASSWORD={newPassword}");
            Console.WriteLine("Пароль успешно изменён!");
            Console.WriteLine("Нажмите любую клавишу для возврата >>>");
            Console.ReadKey(true);
        }
    }
}

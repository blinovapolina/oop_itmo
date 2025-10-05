using System.Text.Json;
using System.Text.Encodings.Web;
using Lab1.Models;


namespace Lab1.Core
{
    public static class Utils
    {
        public static string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo key;

            while (true)
            {
                key = Console.ReadKey(intercept: true);

                if (key.Key == ConsoleKey.Enter) break;

                if (!char.IsControl(key.KeyChar))
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }

                else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password.Substring(0, password.Length - 1);
                    Console.Write("\b \b");
                }

            }

            Console.WriteLine();
            return password;
        }


        public static List<Item> LoadItemsJson(string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine($"Файл {path} не найден. Создаётся пустой список.");
                return [];
            }

            try
            {
                string file = File.ReadAllText(path);
                var items = JsonSerializer.Deserialize<List<Item>>(file);
                return items ?? [];
            }
            catch
            {
                Console.WriteLine("Ошибка чтения json-файла.");
                return [];
            }
        }


        public static void SaveItemsJson(string path, List<Item> items)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine($"Файл {path} не найден.");
            }

            try
            {
                string file = JsonSerializer.Serialize(items, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                });
                File.WriteAllText(path, file);
            }
            catch
            {
                Console.WriteLine("Ошибка сохранения json-файла.");
            }
        }

    }
}

using UniversitySystem.Models;
using UniversitySystem.Services;
using UniversitySystem.Interface;
using System.Text.Json;

namespace UniversitySystem
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.OutputEncoding = System.Text.Encoding.UTF8;
                
                InterfaceUniManager uniManager = new UniManager();
                DataLoader.InitializeData(uniManager);

                var menuService = new MenuService(uniManager);
                menuService.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
                Console.WriteLine("Нажмите любую клавишу для выхода...");
                Console.ReadKey();
            }
        }
    }
}
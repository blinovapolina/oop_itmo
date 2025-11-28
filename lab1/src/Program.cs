using UniversitySystem.Services;
using UniversitySystem.Core;
using UniversitySystem.Interface;

namespace UniversitySystem
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
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
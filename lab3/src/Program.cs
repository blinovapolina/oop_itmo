using System;
using DeliverySystem.Core;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            var demo = new DeliverySystemDemo();
            demo.Run();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}
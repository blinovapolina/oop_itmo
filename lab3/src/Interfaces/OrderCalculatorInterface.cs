using DeliverySystem.Models;

namespace DeliverySystem.Interfaces
{
    public interface IOrderCalculator
    {
        decimal CalculateTotal(Order order);
    }
}
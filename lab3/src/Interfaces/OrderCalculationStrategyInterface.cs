using DeliverySystem.Models;

namespace DeliverySystem.Interfaces
{
    public interface IOrderCalculationStrategy
    {
        decimal CalculateTotal(Order order);
    }
}
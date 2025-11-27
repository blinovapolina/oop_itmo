using InventorySystem.Interfaces;
using InventorySystem.Models;

namespace InventorySystem.Strategies
{
    public class PotionUseStrategy : IUseStrategy
    {
        public void Use(Item item)
        {
            if (item is Potion potion)
            {
                potion.Use();
            }
        }

        public string GetStrategyName() => "Стратегия зелий";
    }
}
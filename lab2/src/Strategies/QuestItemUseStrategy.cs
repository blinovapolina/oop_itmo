using InventorySystem.Interfaces;
using InventorySystem.Models;

namespace InventorySystem.Strategies
{
    public class QuestItemUseStrategy : IUseStrategy
    {
        public void Use(Item item)
        {
            if (item is QuestItem questItem)
            {
                questItem.UseSpecial();
            }
        }

        public string GetStrategyName() => "Стратегия квестовых предметов";
    }
}
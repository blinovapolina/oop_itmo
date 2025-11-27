using InventorySystem.Interfaces;
using InventorySystem.Models;

namespace InventorySystem.Strategies
{
    public class ArmorUseStrategy : IUseStrategy
    {
        public void Use(Item item)
        {
            if (item is Armor armor)
            {
                if (armor.IsEquipped)
                {
                    armor.Unequip();
                }
                else
                {
                    armor.Equip();
                    Console.WriteLine($"Защита усилена: {armor.Name}");
                }
            }
        }

        public string GetStrategyName() => "Стратегия брони";
    }
}
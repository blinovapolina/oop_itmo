using InventorySystem.Interfaces;
using InventorySystem.Models;

namespace InventorySystem.Strategies
{
    public class WeaponUseStrategy : IUseStrategy
    {
        public void Use(Item item)
        {
            if (item is Weapon weapon)
            {
                if (weapon.IsEquipped)
                {
                    weapon.Unequip();
                }
                else
                {
                    weapon.Equip();
                    Console.WriteLine($"Готов к бою с {weapon.Name}!");
                }
            }
        }

        public string GetStrategyName() => "Стратегия оружия";
    }
}
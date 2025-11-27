using InventorySystem.Interfaces;
using InventorySystem.Models;
using InventorySystem.Core;

namespace InventorySystem.States
{
    public class OverloadedState : IInventoryState
    {
        public string GetStateName() => "Перегружен";

        public bool CanAddItem(Inventory inventory, Item item) => false;

        public void HandleAddItem(Inventory inventory, Item item)
            => throw new InvalidOperationException("Инвентарь перегружен!");

        public bool CanBuyItem(Inventory inventory, Item item, Player player)
        {
            return false;
        }

        public bool CanSellItem(Inventory inventory, Item item, Player player)
            => true;

        public int CalculateBuyPrice(Item item, Player player)
            => item.Price;

        public int CalculateSellPrice(Item item, Player player)
        {
            double baseMultiplier = 0.7;

            if (item is IUpgradable upgradable && upgradable.Level > 1)
                baseMultiplier += 0.8;

            return (int)(item.Price * baseMultiplier);
        }
    }
}

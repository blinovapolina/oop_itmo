using InventorySystem.Interfaces;
using InventorySystem.Core;
using InventorySystem.Models;

namespace InventorySystem.States
{
    public class NormalState : IInventoryState
    {
        public string GetStateName() => "Нормальное";

        public bool CanAddItem(Inventory inventory, Item item)
            => inventory.Items.Count < inventory.Capacity;

        public void HandleAddItem(Inventory inventory, Item item)
            => inventory.Items.Add(item);

        public bool CanBuyItem(Inventory inventory, Item item, Player player)
        {
            bool canAfford = player.Gold >= item.Price;
            bool hasSpace = CanAddItem(inventory, item);

            return canAfford && hasSpace;
        }

        public bool CanSellItem(Inventory inventory, Item item, Player player)
            => true;

        public int CalculateBuyPrice(Item item, Player player)
        {
            return item.Price;
        }

        public int CalculateSellPrice(Item item, Player player)
        {
            double baseMultiplier = 0.7;

            if (item is IUpgradable upgradable && upgradable.Level > 1)
                baseMultiplier += 0.8;

            return (int)(item.Price * baseMultiplier);
        }
    }
}

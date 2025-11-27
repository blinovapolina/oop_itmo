using InventorySystem.Interfaces;
using InventorySystem.Models;
using InventorySystem.States;
using InventorySystem.Strategies;

namespace InventorySystem.Core
{
    public class Inventory
    {
        public int Capacity { get; private set; }
        public List<Item> Items { get; private set; }
        public IInventoryState CurrentState { get; private set; }

        private Dictionary<Type, IUseStrategy> _useStrategies;

        public Inventory(int capacity)
        {
            Capacity = capacity;
            Items = new List<Item>();
            CurrentState = new NormalState();

            _useStrategies = new Dictionary<Type, IUseStrategy>
            {
                { typeof(Weapon), new WeaponUseStrategy() },
                { typeof(Armor), new ArmorUseStrategy() },
                { typeof(Potion), new PotionUseStrategy() },
                { typeof(QuestItem), new QuestItemUseStrategy() }
            };
        }

        public bool BuyItem(Item item, Player player)
        {
            if (!CurrentState.CanBuyItem(this, item, player))
            {
                Console.WriteLine($"Нельзя купить {item.Name}!");
                Console.WriteLine($"Причина: {GetBuyRestrictionReason(item, player)}");
                return false;
            }

            int actualPrice = CurrentState.CalculateBuyPrice(item, player);

            if (player.SpendGold(actualPrice))
            {
                AddItem(item);
                Console.WriteLine($"Куплено: {item.Name} за {actualPrice} золота");
                Console.WriteLine($"Остаток: {player.Gold} золота");
                return true;
            }

            return false;
        }

        private string GetBuyRestrictionReason(Item item, Player player)
        {
            if (!CurrentState.CanAddItem(this, item))
                return "Нет места в инвентаре";

            if (player.Gold < item.Price)
                return $"Недостаточно золота. Нужно: {item.Price}, есть: {player.Gold}";

            return "Неизвестная причина";
        }

        public bool SellItem(string itemId, Player player)
        {
            var item = Items.Find(i => i.Id == itemId);
            if (item != null)
            {
                int sellPrice = CalculateSellPrice(item);

                Items.Remove(item);
                player.AddGold(sellPrice);

                Console.WriteLine($"Продано: {item.Name} за {sellPrice} золота");
                Console.WriteLine($"Теперь у вас: {player.Gold} золота");

                CheckState();
                return true;
            }

            Console.WriteLine($"Предмет с ID {itemId} не найден");
            return false;
        }

        private int CalculateSellPrice(Item item)
        {
            if (item is IUpgradable upgradable && upgradable.Level > 1)
            {
                return (int)(item.Price * 0.8);
            }
            return (int)(item.Price * 0.7);
        }

        public bool AddItem(Item item)
        {
            CurrentState.HandleAddItem(this, item);
            return CurrentState.CanAddItem(this, item);
        }

        public bool RemoveItem(string itemId)
        {
            var item = Items.FirstOrDefault(i => i.Id == itemId);
            if (item != null)
            {
                Items.Remove(item);
                Console.WriteLine($"Предмет {item.Name} удален из инвентаря");
                CheckState();
                return true;
            }
            return false;
        }

        public void UseItem(string itemId)
        {
            var item = Items.Find(i => i.Id == itemId);
            if (item != null)
            {
                var itemType = item.GetType();
                if (_useStrategies.ContainsKey(itemType))
                {
                    _useStrategies[itemType].Use(item);

                    if (item is IUsable usable && usable.Quantity == 0)
                    {
                        RemoveItem(itemId);
                    }
                }
                else
                {
                    Console.WriteLine($"Неизвестный тип предмета: {itemType.Name}");
                }
            }
            else
            {
                Console.WriteLine($"Предмет с ID {itemId} не найден");
            }
        }

        public void UpgradeItem(string itemId)
        {
            var item = Items.Find(i => i.Id == itemId) as IUpgradable;
            if (item != null)
            {
                if (item.CanUpgrade())
                {
                    item.Upgrade();
                }
                else
                {
                    Console.WriteLine($"Нельзя улучшить {((Item)item).Name} дальше");
                }
            }
            else
            {
                Console.WriteLine($"Предмет с ID {itemId} нельзя улучшить или не найден");
            }
        }

        public void DisplayInventory(Player player)
        {
            Console.WriteLine($"\n=== ИНВЕНТАРЬ [{Items.Count}/{Capacity}] ===");
            Console.WriteLine($"Состояние: {CurrentState.GetStateName()}");

            var groupedItems = Items.GroupBy(i => i.GetType());

            foreach (var group in groupedItems)
            {
                Console.WriteLine($"\n--- {GetTypeName(group.Key.Name)} ---");
                foreach (var item in group)
                {
                    var equipped = item is IEquipable equipable && equipable.IsEquipped ? "[ЭКИПИРОВАНО]" : "";
                    var quantity = item is IUsable usable ? $" x{usable.Quantity}" : "";
                    var questInfo = item is QuestItem quest ? $" [{quest.QuestItemType}]" : "";

                    var sellPrice = CurrentState.CalculateSellPrice(item, player);

                    Console.WriteLine($"- {item.Id}: {item.Name}{quantity} {equipped}{questInfo}");
                    Console.WriteLine($"  Описание: {item.Description}");
                    Console.WriteLine($"  Цена: {item.Price} золота | Продажа: {sellPrice} золота");
                }
            }
        }

        private string GetTypeName(string typeName)
        {
            if (typeName == "Weapon")
            {
                return "Оружие";
            }
            else if (typeName == "Armor")
            {
                return "Броня";
            }
            else if (typeName == "Potion")
            {
                return "Зелья";
            }
            else if (typeName == "QuestItem")
            {
                return "Квестовые предметы";
            }
            else
            {
                return typeName;
            }
        }

        public void CheckState()
        {
            if (Items.Count >= Capacity)
            {
                CurrentState = new OverloadedState();
            }
            else
            {
                CurrentState = new NormalState();
            }
        }
    }
}
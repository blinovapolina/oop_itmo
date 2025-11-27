using InventorySystem.Services;
using InventorySystem.Models;
using InventorySystem.Interfaces;

namespace InventorySystem.Core
{
    public class Game
    {
        private Player _player;
        private ItemLoader _itemLoader;

        public Game()
        {
            _player = new Player("Герой", 1, 100, 15, 1000);
            _itemLoader = new ItemLoader();
        }

        public void RunGame()
        {
            Console.Clear();

            try
            {
                _itemLoader.LoadItems("Data/items.json");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки предметов: {ex.Message}");
                return;
            }

            Console.WriteLine("=== СИСТЕМА ИНВЕНТАРЯ ===");
            Console.WriteLine($"Начальное золото: {_player.Gold}\n");

            DisplayAllAvailableItems();
            DemonstrateItemBuying();
            DemonstrateItemUsage();
            DemonstrateItemUpgrading();
            DemonstratePlayerActions();
            DemonstrateSelling();
            DemonstrateItemRemoval();
        }

        private void DisplayAllAvailableItems()
        {
            Console.WriteLine("=== ВЕСЬ ДОСТУПНЫЙ ИНВЕНТАРЬ ===\n");

            Console.WriteLine("ОРУЖИЕ:");
            foreach (var weapon in _itemLoader.Weapons)
            {
                Console.WriteLine($"  {weapon.Name} | Урон: {weapon.Damage} | Редкость: {weapon.Rarity} | Цена: {weapon.Price} золота");
                Console.WriteLine($"   ID: {weapon.Id} | Тип: {weapon.WeaponType}");
            }

            Console.WriteLine("\nБРОНЯ:");
            foreach (var armor in _itemLoader.Armors)
            {
                Console.WriteLine($"  {armor.Name} | Защита: {armor.Defense} | Редкость: {armor.Rarity} | Цена: {armor.Price} золота");
                Console.WriteLine($"   ID: {armor.Id} | Тип: {armor.ArmorType} | Слот: {armor.Slot}");
            }

            Console.WriteLine("\nЗЕЛЬЯ:");
            foreach (var potion in _itemLoader.Potions)
            {
                Console.WriteLine($"  {potion.Name} | Лечение: {potion.HealAmount} HP | Редкость: {potion.Rarity} | Цена: {potion.Price} золота");
                Console.WriteLine($"   ID: {potion.Id} | Макс. стаков: {potion.MaxStack}");
            }

            Console.WriteLine("\nКВЕСТОВЫЕ ПРЕДМЕТЫ:");
            foreach (var questItem in _itemLoader.QuestItems)
            {
                Console.WriteLine($"  {questItem.Name} | Тип: {questItem.QuestItemType} | Редкость: {questItem.Rarity} | Цена: {questItem.Price} золота");
                Console.WriteLine($"   ID: {questItem.Id} | Эффект: {questItem.EffectDescription}");
            }

            Console.WriteLine($"\nВаш бюджет: {_player.Gold} золота");
            Console.WriteLine("═══════════════════════════════════════\n");
        }

        private void DemonstrateItemBuying()
        {
            Console.WriteLine("\n--- ПОКУПКА ПРЕДМЕТОВ ---");
            Console.WriteLine($"Текущее золото: {_player.Gold}");

            var random = new Random();
            int itemsBought = 0;

            if (_itemLoader.Weapons.Count > 0)
            {
                var weapon = _itemLoader.Weapons[random.Next(_itemLoader.Weapons.Count)];
                if (_player.Inventory.BuyItem(weapon, _player))
                    itemsBought++;
            }

            if (_itemLoader.Armors.Count > 0)
            {
                var armor = _itemLoader.Armors[random.Next(_itemLoader.Armors.Count)];
                if (_player.Inventory.BuyItem(armor, _player))
                    itemsBought++;
            }

            foreach (var potion in _itemLoader.Potions.Take(2))
            {
                if (_player.Inventory.BuyItem(potion, _player))
                    itemsBought++;
            }

            foreach (var questItem in _itemLoader.QuestItems)
            {
                if (_player.Inventory.BuyItem(questItem, _player))
                    itemsBought++;
            }

            if (itemsBought == 0)
            {
                Console.WriteLine("Игрок не смог купить ни одного предмета!");
            }

            Console.WriteLine("\n--- ИНВЕНТАРЬ ПОСЛЕ ПОКУПОК ---");
            _player.Inventory.DisplayInventory(_player);
        }

        private void DemonstrateSelling()
        {
            Console.WriteLine("\n--- ПРОДАЖА ПРЕДМЕТОВ ---");
            Console.WriteLine($"Золото до продажи: {_player.Gold}");

            var items = _player.Inventory.Items;
            if (items.Count > 0)
            {
                var itemToSell = items[0];
                _player.Inventory.SellItem(itemToSell.Id, _player);
            }
            else
            {
                Console.WriteLine("В инвентаре нет предметов для продажи");
            }

            Console.WriteLine("\n--- ИНВЕНТАРЬ ПОСЛЕ ПРОДАЖИ ---");
            _player.Inventory.DisplayInventory(_player);
        }

        private void DemonstrateItemUsage()
        {
            Console.WriteLine("\n--- ИСПОЛЬЗОВАНИЕ ПРЕДМЕТОВ ---");

            var items = _player.Inventory.Items;
            if (items.Count > 0)
            {
                var weapon = items.Find(i => i is Weapon);
                if (weapon != null)
                {
                    _player.Inventory.UseItem(weapon.Id);
                }

                var armor = items.Find(i => i is Armor);
                if (armor != null)
                {
                    _player.Inventory.UseItem(armor.Id);
                }

                var potion = items.Find(i => i is Potion);
                if (potion != null)
                {
                    _player.Inventory.UseItem(potion.Id);
                }

                var questItem = items.Find(i => i is QuestItem);
                if (questItem != null)
                {
                    _player.Inventory.UseItem(questItem.Id);
                }
            }
            else
            {
                Console.WriteLine("В инвентаре нет предметов для использования");
            }
        }

        private void DemonstrateItemUpgrading()
        {
            Console.WriteLine("\n--- УЛУЧШЕНИЕ ПРЕДМЕТОВ ---");

            var upgradableItems = new List<IUpgradable>();
            foreach (var item in _player.Inventory.Items)
            {
                if (item is IUpgradable)
                {
                    upgradableItems.Add((IUpgradable)item);
                }
            }

            if (upgradableItems.Count > 0)
            {
                var itemToUpgrade = upgradableItems[0];

                if (itemToUpgrade != null && itemToUpgrade is Item baseItem)
                {
                    _player.Inventory.UpgradeItem(baseItem.Id);
                    _player.Inventory.UpgradeItem(baseItem.Id);
                }
                else
                {
                    Console.WriteLine("Ошибка: не удалось преобразовать предмет");
                }
            }
            else
            {
                Console.WriteLine("В инвентаре нет предметов для улучшения");
            }
        }

        private void DemonstratePlayerActions()
        {
            Console.WriteLine("\n--- СОСТОЯНИЕ ИГРОКА ---");
            Console.WriteLine($"Текущее золото: {_player.Gold}");
            _player.Heal(20);
            _player.TakeDamage(15);

            Console.WriteLine("\n--- ФИНАЛЬНЫЙ ИНВЕНТАРЬ ---");
            _player.Inventory.DisplayInventory(_player);
        }

        private void DemonstrateItemRemoval()
        {
            Console.WriteLine("\n--- ТЕСТИРОВАНИЕ УДАЛЕНИЯ ---");

            var items = _player.Inventory.Items;
            if (items.Count > 1)
            {
                var itemToRemove = items[1];
                _player.Inventory.RemoveItem(itemToRemove.Id);
            }
            else if (items.Count == 1)
            {
                _player.Inventory.RemoveItem(items[0].Id);
            }
            else
            {
                Console.WriteLine("В инвентаре нет предметов для удаления");
            }

            Console.WriteLine("\n--- ИНВЕНТАРЬ ПОСЛЕ УДАЛЕНИЯ ---");
            _player.Inventory.DisplayInventory(_player);

            Console.WriteLine("\n=== ПОКА-ПОКА! ===");
        }
    }
}
using System.Text.Json;
using System.Text.Json.Serialization;
using InventorySystem.Enums;
using InventorySystem.Models;
using InventorySystem.Factories;

namespace InventorySystem.Services
{
    public class ItemLoader
    {
        public List<Weapon> Weapons { get; private set; } = new();
        public List<Armor> Armors { get; private set; } = new();
        public List<Potion> Potions { get; private set; } = new();
        public List<QuestItem> QuestItems { get; private set; } = new();

        private static JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        };

        public void LoadItems(string jsonFilePath)
        {
            if (!File.Exists(jsonFilePath))
                throw new FileNotFoundException($"JSON файл не найден: {jsonFilePath}");

            var json = File.ReadAllText(jsonFilePath);
            var data = JsonSerializer.Deserialize<ItemData>(json, _jsonOptions);

            if (data == null) throw new InvalidOperationException("Не удалось загрузить данные предметов");

            LoadWeapons(data.Weapons);
            LoadArmors(data.Armors);
            LoadPotions(data.Potions);
            LoadQuestItems(data.QuestItems);
        }

        private void LoadWeapons(List<WeaponTemplate> weaponsData)
        {
            var factory = new ItemFactory();

            foreach (var data in weaponsData)
            {
                try
                {
                    var weapon = factory.CreateWeapon(data.Name, data.WeaponType, data.Rarity);
                    Weapons.Add(weapon);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка создания оружия {data.Name}: {ex.Message}");
                }
            }
        }

        private void LoadArmors(List<ArmorTemplate> armorsData)
        {
            var factory = new ItemFactory();

            foreach (var data in armorsData)
            {
                try
                {
                    var armor = factory.CreateArmor(data.Name, data.ArmorType, data.Rarity);
                    Armors.Add(armor);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка создания брони {data.Name}: {ex.Message}");
                }
            }
        }

        private void LoadPotions(List<PotionTemplate> potionsData)
        {
            var factory = new ItemFactory();

            foreach (var data in potionsData)
            {
                try
                {
                    var potion = factory.CreatePotion(data.Name, data.HealAmount, data.Rarity);
                    Potions.Add(potion);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка создания зелья {data.Name}: {ex.Message}");
                }
            }
        }

        private void LoadQuestItems(List<QuestItemTemplate> questItemsData)
        {
            var factory = new ItemFactory();

            foreach (var data in questItemsData)
            {
                try
                {
                    var questItem = factory.CreateQuestItem(data.Name, data.QuestType, data.Rarity, data.EffectDescription);
                    QuestItems.Add(questItem);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка создания квестового предмета {data.Name}: {ex.Message}");
                }
            }
        }
    }

    public class ItemData
    {
        public List<WeaponTemplate> Weapons { get; set; } = new();
        public List<ArmorTemplate> Armors { get; set; } = new();
        public List<PotionTemplate> Potions { get; set; } = new();
        public List<QuestItemTemplate> QuestItems { get; set; } = new();
    }

    public class WeaponTemplate
    {
        public string Name { get; set; } = string.Empty;
        public WeaponType WeaponType { get; set; }
        public Rarity Rarity { get; set; }
    }

    public class ArmorTemplate
    {
        public string Name { get; set; } = string.Empty;
        public ArmorType ArmorType { get; set; }
        public Rarity Rarity { get; set; }
    }

    public class PotionTemplate
    {
        public string Name { get; set; } = string.Empty;
        public Rarity Rarity { get; set; }
        public int HealAmount { get; set; }
    }

    public class QuestItemTemplate
    {
        public string Name { get; set; } = string.Empty;
        public QuestItemType QuestType { get; set; }
        public Rarity Rarity { get; set; }
        public string EffectDescription { get; set; } = string.Empty;
    }
}
using InventorySystem.Builders;
using InventorySystem.Enums;
using InventorySystem.Interfaces;
using InventorySystem.Models;

namespace InventorySystem.Factories
{
    public class ItemFactory : IItemFactory
    {
        private int _weaponCount = 0;
        private int _armorCount = 0;
        private int _potionCount = 0;
        private int _questCount = 0;

        public Weapon CreateWeapon(string name, WeaponType weaponType, Rarity rarity)
        {
            var damage = CalculateWeaponDamage(weaponType, rarity);
            var price = CalculateWeaponPrice(rarity, damage);

            var builder = new ItemBuilder();
            builder.SetId($"{++_weaponCount}");
            builder.SetName(name);
            builder.SetWeaponType(weaponType);
            builder.SetRarity(rarity);
            builder.SetDamage(damage);
            builder.SetPrice(price);

            return builder.BuildWeapon();
        }

        public Armor CreateArmor(string name, ArmorType armorType, Rarity rarity)
        {
            var defense = CalculateArmorDefense(armorType, rarity);
            var slot = GetSlotForArmorType(armorType);
            var price = CalculateArmorPrice(rarity, defense);

            var builder = new ItemBuilder();
            builder.SetId($"{++_armorCount}");
            builder.SetName(name);
            builder.SetArmorType(armorType);
            builder.SetArmorSlot(slot);
            builder.SetRarity(rarity);
            builder.SetDefense(defense);
            builder.SetPrice(price);

            return builder.BuildArmor();
        }

        public Potion CreatePotion(string name, int healAmount, Rarity rarity)
        {
            var price = CalculatePotionPrice(rarity, healAmount);
            var maxStack = CalculatePotionStackSize(rarity);

            var builder = new ItemBuilder();
            builder.SetId($"{++_potionCount}");
            builder.SetName(name);
            builder.SetRarity(rarity);
            builder.SetHealAmount(healAmount);
            builder.SetPrice(price);
            builder.SetMaxStack(maxStack);

            return builder.BuildPotion();
        }

        public QuestItem CreateQuestItem(string name, QuestItemType questType, Rarity rarity, string effectDescription)
        {
            var price = CalculateQuestItemPrice(rarity);

            var builder = new ItemBuilder();
            builder.SetId($"{++_questCount}");
            builder.SetName(name);
            builder.SetQuestType(questType);
            builder.SetRarity(rarity);
            builder.SetPrice(price);
            builder.SetEffectDescription(effectDescription);

            return builder.BuildQuestItem();
        }

        private int CalculateWeaponDamage(WeaponType type, Rarity rarity)
        {
            int weaponDamage;

            if (type == WeaponType.Sword)
                weaponDamage = 15;
            else if (type == WeaponType.Bow)
                weaponDamage = 12;
            else if (type == WeaponType.Axe)
                weaponDamage = 18;
            else
                weaponDamage = 10;

            return weaponDamage * ((int)rarity + 1);
        }

        private int CalculateArmorDefense(ArmorType type, Rarity rarity)
        {
            int weaponDefense;

            if (type == ArmorType.Helmet)
                weaponDefense = 5;
            else if (type == ArmorType.Chest)
                weaponDefense = 10;
            else if (type == ArmorType.Boots)
                weaponDefense = 4;
            else if (type == ArmorType.Shield)
                weaponDefense = 8;
            else
                weaponDefense = 5;

            return weaponDefense * ((int)rarity + 1);
        }

        private EquipmentSlot GetSlotForArmorType(ArmorType armorType)
        {
            if (armorType == ArmorType.Helmet)
                return EquipmentSlot.Head;
            else if (armorType == ArmorType.Chest)
                return EquipmentSlot.Body;
            else if (armorType == ArmorType.Boots)
                return EquipmentSlot.Feet;
            else if (armorType == ArmorType.Shield)
                return EquipmentSlot.Hands;
            else
                return EquipmentSlot.Body;
        }

        private int CalculateWeaponPrice(Rarity rarity, int damage) => damage * 10 * ((int)rarity + 1);
        private int CalculateArmorPrice(Rarity rarity, int defense) => defense * 8 * ((int)rarity + 1);
        private int CalculatePotionPrice(Rarity rarity, int healAmount) => healAmount * 2 * ((int)rarity + 1);
        private int CalculateQuestItemPrice(Rarity rarity) => 50 * ((int)rarity + 1);
        private int CalculatePotionStackSize(Rarity rarity) => 10 - (int)rarity * 2;
    }
}
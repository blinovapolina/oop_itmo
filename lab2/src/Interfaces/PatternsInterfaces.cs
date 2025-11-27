using InventorySystem.Enums;
using InventorySystem.Models;
using InventorySystem.Core;

namespace InventorySystem.Interfaces
{
    public interface IItemBuilder
    {
        void SetId(string id);
        void SetName(string name);
        void SetRarity(Rarity rarity);
        void SetPrice(int price);

        void SetWeaponType(WeaponType weaponType);
        void SetDamage(int damage);

        void SetArmorType(ArmorType armorType);
        void SetArmorSlot(EquipmentSlot slot);
        void SetDefense(int defense);

        void SetHealAmount(int healAmount);
        void SetMaxStack(int maxStack);

        void SetQuestType(QuestItemType questType);
        void SetEffectDescription(string description);

        Weapon BuildWeapon();
        Armor BuildArmor();
        Potion BuildPotion();
        QuestItem BuildQuestItem();
    }

    public interface IInventoryState
    {
        bool CanAddItem(Inventory inventory, Item item);
        void HandleAddItem(Inventory inventory, Item item);
        string GetStateName();

        bool CanBuyItem(Inventory inventory, Item item, Player player);
        bool CanSellItem(Inventory inventory, Item item, Player player);
        int CalculateBuyPrice(Item item, Player player);
        int CalculateSellPrice(Item item, Player player);
    }

    public interface IUseStrategy
    {
        void Use(Item item);
        string GetStrategyName();
    }

    public interface IItemFactory
    {
        Weapon CreateWeapon(string name, WeaponType weaponType, Rarity rarity);
        Armor CreateArmor(string name, ArmorType armorType, Rarity rarity);
        Potion CreatePotion(string name, int healAmount, Rarity rarity);
        QuestItem CreateQuestItem(string name, QuestItemType questType, Rarity rarity, string effectDescription);
    }
}
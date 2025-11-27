using InventorySystem.Enums;
using InventorySystem.Models;
using InventorySystem.Interfaces;

namespace InventorySystem.Builders
{
    public class ItemBuilder : IItemBuilder
    {
        private string? _id;
        private string? _name;
        private Rarity _rarity = Rarity.Common;
        private int _price = 10;

        private WeaponType _weaponType;
        private int _damage;

        private ArmorType _armorType;
        private EquipmentSlot _armorSlot;
        private int _defense;

        private int _healAmount;
        private int _maxStack = 5;

        private QuestItemType _questType;
        private string? _effectDescription;
        private int _questMaxStack = 1;

        public void SetId(string id) { _id = id; }
        public void SetName(string name) { _name = name; }
        public void SetRarity(Rarity rarity) { _rarity = rarity; }
        public void SetPrice(int price) { _price = price; }

        public void SetWeaponType(WeaponType weaponType) { _weaponType = weaponType; }
        public void SetDamage(int damage) { _damage = damage; }

        public void SetArmorType(ArmorType armorType) { _armorType = armorType; }
        public void SetArmorSlot(EquipmentSlot slot) { _armorSlot = slot; }
        public void SetDefense(int defense) { _defense = defense; }

        public void SetHealAmount(int healAmount) { _healAmount = healAmount; }
        public void SetMaxStack(int maxStack) { _maxStack = maxStack; }

        public void SetQuestType(QuestItemType questType) { _questType = questType; }
        public void SetEffectDescription(string description) { _effectDescription = description; }
        public void SetQuestMaxStack(int maxStack) { _questMaxStack = maxStack; }

        public Weapon BuildWeapon()
        {
            ValidateRequiredFields();
            ValidateWeaponFields();

            return new Weapon(_id!, _name!, _weaponType, _rarity, _damage, _price);
        }

        public Armor BuildArmor()
        {
            ValidateRequiredFields();
            ValidateArmorFields();

            return new Armor(_id!, _name!, _armorType, _rarity, _defense, _armorSlot, _price);
        }

        public Potion BuildPotion()
        {
            ValidateRequiredFields();
            ValidatePotionFields();

            return new Potion(_id!, _name!, _rarity, _healAmount, _price, _maxStack);
        }

        public QuestItem BuildQuestItem()
        {
            ValidateRequiredFields();
            ValidateQuestFields();

            return new QuestItem(
                _id!,
                _name!,
                _questType,
                _rarity,
                _effectDescription ?? "Без описания",
                _price,
                _questMaxStack);
        }

        private void ValidateRequiredFields()
        {
            if (string.IsNullOrEmpty(_id))
                throw new InvalidOperationException("ID обязателен");
            if (string.IsNullOrEmpty(_name))
                throw new InvalidOperationException("Name обязателен");
        }

        private void ValidateWeaponFields()
        {
            if (_damage <= 0)
                throw new InvalidOperationException("Damage должен быть положительным");
        }

        private void ValidateArmorFields()
        {
            if (_defense < 0)
                throw new InvalidOperationException("Defense не может быть отрицательным");
        }

        private void ValidatePotionFields()
        {
            if (_healAmount <= 0)
                throw new InvalidOperationException("HealAmount должен быть положительным");
            if (_maxStack <= 0)
                throw new InvalidOperationException("MaxStack должен быть положительным");
        }

        private void ValidateQuestFields()
        {
            if (_questMaxStack <= 0)
                throw new InvalidOperationException("QuestMaxStack должен быть положительным");
        }
    }
}
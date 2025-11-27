using Xunit;
using InventorySystem.Enums;
using InventorySystem.Models;
using InventorySystem.Builders;

namespace InventorySystem.Tests.UnitTests
{
    public class ItemTests
    {
        [Fact]
        public void Weapon_Creation_ShouldSetCorrectProperties()
        {
            // Arrange & Act
            var weapon = new Weapon("W001", "Тестовый меч", WeaponType.Sword, Rarity.Common, 10, 100);

            // Assert
            Assert.Equal("W001", weapon.Id);
            Assert.Equal("Тестовый меч", weapon.Name);
            Assert.Equal(WeaponType.Sword, weapon.WeaponType);
            Assert.Equal(Rarity.Common, weapon.Rarity);
            Assert.Equal(10, weapon.Damage);
            Assert.Equal(100, weapon.Price);
            Assert.Equal(ItemType.Weapon, weapon.Type);
            Assert.False(weapon.IsEquipped);
        }

        [Fact]
        public void Weapon_Upgrade_ShouldIncreaseDamageAndPrice()
        {
            // Arrange
            var weapon = new Weapon("W001", "Тестовый меч", WeaponType.Sword, Rarity.Common, 100, 100);

            // Act
            weapon.Upgrade();

            // Assert
            Assert.Equal(2, weapon.Level);
            Assert.Equal(130, weapon.Damage);
            Assert.Equal(150, weapon.Price);
        }

        [Fact]
        public void Weapon_CanUpgrade_ShouldReturnFalseAtMaxLevel()
        {
            // Arrange
            var weapon = new Weapon("W001", "Тестовый меч", WeaponType.Sword, Rarity.Common, 10, 100);

            // Act
            for (int i = 0; i < 4; i++)
            {
                weapon.Upgrade();
            }

            // Assert
            Assert.Equal(5, weapon.Level);
            Assert.False(weapon.CanUpgrade());
        }

        [Fact]
        public void Armor_Equip_ShouldSetIsEquippedToTrue()
        {
            // Arrange
            var armor = new Armor("A001", "Тестовый шлем", ArmorType.Helmet, Rarity.Common, 5, EquipmentSlot.Head, 50);

            // Act
            armor.Equip();

            // Assert
            Assert.True(armor.IsEquipped);
        }

        [Fact]
        public void Armor_Unequip_ShouldSetIsEquippedToFalse()
        {
            // Arrange
            var armor = new Armor("A001", "Тестовый шлем", ArmorType.Helmet, Rarity.Common, 5, EquipmentSlot.Head, 50);
            armor.Equip();

            // Act
            armor.Unequip();

            // Assert
            Assert.False(armor.IsEquipped);
        }

        [Fact]
        public void Potion_Use_ShouldDecreaseQuantity()
        {
            // Arrange
            var potion = new Potion("P001", "Тестовое зелье", Rarity.Common, 20, 25, 3);
            potion.Quantity = 2;

            // Act
            potion.Use();

            // Assert
            Assert.Equal(1, potion.Quantity);
        }

        [Fact]
        public void Potion_Use_WhenQuantityZero_ShouldNotDecreaseQuantity()
        {
            // Arrange
            var potion = new Potion("P001", "Тестовое зелье", Rarity.Common, 20, 25, 3);
            potion.Quantity = 0;

            // Act
            potion.Use();

            // Assert
            Assert.Equal(0, potion.Quantity);
        }

        [Fact]
        public void QuestItem_UseSpecial_ShouldDecreaseQuantity()
        {
            // Arrange
            var questItem = new QuestItem("Q001", "Тестовый ключ", QuestItemType.Key, Rarity.Common, "Тестовый эффект", 75);

            // Act
            questItem.UseSpecial();

            // Assert
            Assert.Equal(0, questItem.Quantity);
        }

        [Fact]
        public void ItemBuilder_ShouldBuildWeaponCorrectly()
        {
            // Arrange & Act
            var builder = new ItemBuilder();
            builder.SetId("W001");
            builder.SetName("Тестовый меч");
            builder.SetWeaponType(WeaponType.Sword);
            builder.SetRarity(Rarity.Common);
            builder.SetDamage(10);
            builder.SetPrice(100);
            var weapon = builder.BuildWeapon();

            // Assert
            Assert.IsType<Weapon>(weapon);
            Assert.Equal("W001", weapon.Id);
            Assert.Equal("Тестовый меч", weapon.Name);
            Assert.Equal(10, ((Weapon)weapon).Damage);
        }

        [Fact]
        public void ItemBuilder_ShouldBuildArmorCorrectly()
        {
            // Arrange & Act
            var builder = new ItemBuilder();
            builder.SetId("A001");
            builder.SetName("Тестовый шлем");
            builder.SetArmorType(ArmorType.Helmet);
            builder.SetRarity(Rarity.Common);
            builder.SetDefense(5);
            builder.SetArmorSlot(EquipmentSlot.Head);
            builder.SetPrice(50);
            var armor = builder.BuildArmor();

            // Assert
            Assert.IsType<Armor>(armor);
            Assert.Equal("A001", armor.Id);
            Assert.Equal("Тестовый шлем", armor.Name);
            Assert.Equal(5, ((Armor)armor).Defense);
            Assert.Equal(EquipmentSlot.Head, ((Armor)armor).Slot);
        }
    }
}
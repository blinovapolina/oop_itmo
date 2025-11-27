using Xunit;
using InventorySystem.Models;
using InventorySystem.Enums;

namespace InventorySystem.Tests.UnitTests
{
    public class PlayerTests
    {
        [Fact]
        public void Player_Creation_ShouldSetCorrectProperties()
        {
            // Arrange & Act
            var player = new Player("Тестовый игрок", 5, 150, 20);

            // Assert
            Assert.Equal("Тестовый игрок", player.Name);
            Assert.Equal(5, player.Level);
            Assert.Equal(150, player.Health);
            Assert.Equal(150, player.MaxHealth);
            Assert.NotNull(player.Inventory);
            Assert.Equal(20, player.Inventory.Capacity);
        }

        [Fact]
        public void Player_Heal_ShouldIncreaseHealth()
        {
            // Arrange
            var player = new Player("Тестовый игрок", 1, 100, 10);
            using (var consoleOutput = new ConsoleOutput())
            {
                player.TakeDamage(30);
            }

            // Act
            player.Heal(20);

            // Assert
            Assert.Equal(90, player.Health);
        }

        [Fact]
        public void Player_Heal_ShouldNotExceedMaxHealth()
        {
            // Arrange
            var player = new Player("Тестовый игрок", 1, 100, 10);
            using (var consoleOutput = new ConsoleOutput())
            {
                player.TakeDamage(20);
            }

            // Act
            player.Heal(30);

            // Assert
            Assert.Equal(100, player.Health);
        }

        [Fact]
        public void Player_TakeDamage_ShouldDecreaseHealth()
        {
            // Arrange
            var player = new Player("Тестовый игрок", 1, 100, 10);

            // Act
            using (var consoleOutput = new ConsoleOutput())
            {
                player.TakeDamage(25);
            }

            // Assert
            Assert.Equal(75, player.Health);
        }

        [Fact]
        public void Player_TakeDamage_ShouldNotGoBelowZero()
        {
            // Arrange
            var player = new Player("Тестовый игрок", 1, 100, 10);

            // Act
            using (var consoleOutput = new ConsoleOutput())
            {
                player.TakeDamage(150);
            }

            // Assert
            Assert.Equal(0, player.Health);
        }

        [Fact]
        public void Player_TakeDamage_WhenHealthZero_ShouldShowDefeatedMessage()
        {
            // Arrange
            var player = new Player("Тестовый игрок", 1, 100, 10);

            // Act
            using (var consoleOutput = new ConsoleOutput())
            {
                player.TakeDamage(100);

                // Assert
                Assert.Contains("повержен", consoleOutput.GetOutput());
            }
        }

        [Fact]
        public void Player_CanAddItemsToInventory()
        {
            // Arrange
            var player = new Player("Тестовый игрок", 1, 100, 5);
            var weapon = new Weapon("W001", "Тестовый меч", WeaponType.Sword, Rarity.Common, 10, 100);

            // Act
            var result = player.Inventory.AddItem(weapon);

            // Assert
            Assert.True(result);
            Assert.Single(player.Inventory.Items);
        }
    }
}
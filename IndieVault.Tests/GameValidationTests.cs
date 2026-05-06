using IndieVault.Models;
using System.ComponentModel.DataAnnotations;

namespace IndieVault.Tests
{
    public class GameValidationTests
    {
        private static IList<ValidationResult> ValidateModel(object model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model);
            Validator.TryValidateObject(model, context, results, validateAllProperties: true);
            return results;
        }

        [Fact]
        public void Game_ValidData_PassesValidation()
        {
            var game = new Game
            {
                Title = "Hollow Knight",
                Description = "A masterpiece.",
                Price = 14.99m,
                ReleaseDate = DateTime.Now,
                EngineId = 1,
                GenreId = 1,
                DeveloperId = "dev-123",
                CoverImagePath = "/images/hk.jpg",
                DownloadLink = "https://indievault.com/download/hk"
            };
            var results = ValidateModel(game);

            Assert.Empty(results);
        }

        [Fact]
        public void Game_MissingRequiredFields_FailsValidation()
        {
            var game = new Game { Price = 10m };

            var results = ValidateModel(game);

            Assert.Contains(results, r => r.MemberNames.Contains("Title"));
            Assert.Contains(results, r => r.MemberNames.Contains("Description"));
        }

        [Theory]
        [InlineData(0.00)]
        [InlineData(-5.00)]
        public void Game_InvalidPrice_FailsValidation(decimal invalidPrice)
        {
            var game = new Game
            {
                Title = "Valid Title",
                Description = "Valid Description",
                Price = invalidPrice,
                CoverImagePath = "path.jpg",
                DownloadLink = "link.com"
            };

            var results = ValidateModel(game);

            Assert.Contains(results, r => r.MemberNames.Contains("Price"));
        }

        [Fact]
        public void Game_TitleExceeds100Chars_FailsValidation()
        {
            // Arrange
            var game = new Game
            {
                Title = new string('A', 101), // Exceeds [StringLength(100)]
                Description = "Valid description",
                Price = 19.99m,
                CoverImagePath = "test.jpg",
                DownloadLink = "test.com"
            };

            // Act
            var results = ValidateModel(game);

            // Assert
            // Check that at least one validation error points specifically to the Title property
            Assert.Contains(results, r => r.MemberNames.Contains(nameof(Game.Title)));
        }

        [Fact]
        public void Game_DescriptionExceeds500Chars_FailsValidation()
        {
            // Arrange
            var game = new Game
            {
                Title = "Valid Title",
                Description = new string('B', 501), // Exceeds [StringLength(500)]
                Price = 19.99m,
                CoverImagePath = "test.jpg",
                DownloadLink = "test.com"
            };

            // Act
            var results = ValidateModel(game);

            // Assert
            Assert.Contains(results, r => r.MemberNames.Contains(nameof(Game.Description)));
        }
    }
}

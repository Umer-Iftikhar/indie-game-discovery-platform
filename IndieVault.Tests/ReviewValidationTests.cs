using IndieVault.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndieVault.Tests
{
    public class ReviewValidationTests
    {
        private List<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model); // Cleaner: using default params
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }

        [Fact]
        public void Review_ValidReview_PassesValidation()
        {
            var review = new Review
            {
                Rating = 4,
                Comment = "Great game!",
                GameId = 1,
                UserId = "user-123"
            };

            var results = ValidateModel(review);

            Assert.Empty(results);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(6)]
        [InlineData(-1)]
        public void Review_InvalidRatingRange_FailsValidation(int invalidRating)
        {
            var review = new Review
            {
                Rating = invalidRating,
                GameId = 1,
                UserId = "user-123"
            };

            var results = ValidateModel(review);

            Assert.Contains(results, r => r.MemberNames.Contains(nameof(Review.Rating)));
        }

        [Fact]
        public void Review_CommentExceeds1000Chars_FailsValidation()
        {
            var review = new Review
            {
                Rating = 5,
                Comment = new string('C', 1001),
                GameId = 1,
                UserId = "user-123"
            };

            var results = ValidateModel(review);

            Assert.Contains(results, r => r.MemberNames.Contains(nameof(Review.Comment)));
        }
    }
}

using RecipeMeal.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeMeal.Core.Interfaces.Services
{
	public interface IReviewService
	{
		Task<Review> AddReviewAsync(Review review);
		Task<IEnumerable<Review>> GetReviewsByRecipeIdAsync(int recipeId);
		Task<IEnumerable<Review>> GetAllReviewsAsync();
		Task<double> GetAverageRatingAsync(int recipeId);
		Task<Review> UpdateReviewAsync(int reviewId, string username, Review updatedReview);
		Task<bool> DeleteReviewAsync(int reviewId, string username);
	}
}

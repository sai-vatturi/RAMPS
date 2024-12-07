using Microsoft.EntityFrameworkCore;
using RecipeMeal.Core.Entities;
using RecipeMeal.Core.Interfaces.Services;
using RecipeMeal.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeMeal.Infrastructure.Services
{
	public class ReviewService : IReviewService
	{
		private readonly RecipeMealDbContext _dbContext;

		public ReviewService(RecipeMealDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<Review> AddReviewAsync(Review review)
		{
			var recipe = await _dbContext.Recipes.FindAsync(review.RecipeId);
			if (recipe == null)
				throw new KeyNotFoundException("Recipe not found.");

			review.CreatedAt = DateTime.UtcNow;
			_dbContext.Reviews.Add(review);
			await _dbContext.SaveChangesAsync();
			return review;
		}

		public async Task<IEnumerable<Review>> GetReviewsByRecipeIdAsync(int recipeId)
		{
			var reviews = await _dbContext.Reviews
				.Where(r => r.RecipeId == recipeId)
				.OrderByDescending(r => r.CreatedAt)
				.ToListAsync();

			return reviews;
		}

		public async Task<IEnumerable<Review>> GetAllReviewsAsync()
		{
			return await _dbContext.Reviews
				.OrderByDescending(r => r.CreatedAt)
				.ToListAsync();
		}

		public async Task<double> GetAverageRatingAsync(int recipeId)
		{
			var recipe = await _dbContext.Recipes.FindAsync(recipeId);
			if (recipe == null)
				throw new KeyNotFoundException("Recipe not found.");

			var averageRating = await _dbContext.Reviews
				.Where(r => r.RecipeId == recipeId)
				.AverageAsync(r => (double?)r.Rating);

			return averageRating ?? 0;
		}

		public async Task<Review> UpdateReviewAsync(int reviewId, string username, Review updatedReview)
		{
			var review = await _dbContext.Reviews.FindAsync(reviewId);
			if (review == null)
				throw new KeyNotFoundException("Review not found.");

			if (review.UserName != username)
				throw new UnauthorizedAccessException("You are not authorized to update this review.");

			review.Rating = updatedReview.Rating;
			review.Comment = updatedReview.Comment;
			await _dbContext.SaveChangesAsync();

			return review;
		}

		public async Task<bool> DeleteReviewAsync(int reviewId, string username)
		{
			var review = await _dbContext.Reviews.FindAsync(reviewId);
			if (review == null)
				throw new KeyNotFoundException("Review not found.");

			if (review.UserName != username)
				throw new UnauthorizedAccessException("You are not authorized to delete this review.");

			_dbContext.Reviews.Remove(review);
			await _dbContext.SaveChangesAsync();

			return true;
		}
	}
}

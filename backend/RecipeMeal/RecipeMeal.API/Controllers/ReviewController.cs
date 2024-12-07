using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeMeal.Core.DTOs; // Add the DTO namespace
using RecipeMeal.Core.DTOs.Review;
using RecipeMeal.Core.Entities;
using RecipeMeal.Core.Interfaces.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RecipeMeal.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ReviewController : ControllerBase
	{
		private readonly IReviewService _reviewService;

		public ReviewController(IReviewService reviewService)
		{
			_reviewService = reviewService;
		}

		private string GetUsernameFromToken()
		{
			return User.FindFirstValue(ClaimTypes.Name);
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> AddReview([FromBody] ReviewDto reviewDto)
		{
			try
			{
				// Create Review entity from DTO
				var review = new Review
				{
					RecipeId = reviewDto.RecipeId,
					Rating = reviewDto.Rating,
					Comment = reviewDto.Comment,
					UserName = GetUsernameFromToken()
				};

				var addedReview = await _reviewService.AddReviewAsync(review);
				return Ok(addedReview);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(new { message = ex.Message });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpGet("recipe/{recipeId}")]
		public async Task<IActionResult> GetReviewsByRecipeId(int recipeId)
		{
			var reviews = await _reviewService.GetReviewsByRecipeIdAsync(recipeId);
			return Ok(reviews);
		}

		[HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetAllReviews()
		{
			var reviews = await _reviewService.GetAllReviewsAsync();
			return Ok(reviews);
		}

		[HttpGet("average-rating/{recipeId}")]
		public async Task<IActionResult> GetAverageRating(int recipeId)
		{
			try
			{
				var averageRating = await _reviewService.GetAverageRatingAsync(recipeId);
				return Ok(new { AverageRating = averageRating });
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(new { message = ex.Message });
			}
		}

		[HttpPut("{reviewId}")]
		[Authorize]
		public async Task<IActionResult> UpdateReview(int reviewId, [FromBody] ReviewDto updatedReviewDto)
		{
			try
			{
				var username = GetUsernameFromToken();
				var review = new Review
				{
					ReviewId = reviewId,
					RecipeId = updatedReviewDto.RecipeId,
					Rating = updatedReviewDto.Rating,
					Comment = updatedReviewDto.Comment,
					UserName = username
				};
				var updatedReview = await _reviewService.UpdateReviewAsync(reviewId, username, review);
				return Ok(updatedReview);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(new { message = ex.Message });
			}
			catch (UnauthorizedAccessException ex)
			{
				return Forbid(ex.Message);
			}
		}

		[HttpDelete("{reviewId}")]
		[Authorize]
		public async Task<IActionResult> DeleteReview(int reviewId)
		{
			try
			{
				var username = GetUsernameFromToken();
				var isDeleted = await _reviewService.DeleteReviewAsync(reviewId, username);
				return Ok(new { message = "Review deleted successfully." });
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(new { message = ex.Message });
			}
			catch (UnauthorizedAccessException ex)
			{
				return Forbid(ex.Message);
			}
		}
	}
}

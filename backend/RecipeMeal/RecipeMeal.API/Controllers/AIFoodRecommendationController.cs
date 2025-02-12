using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeMeal.Core.Interfaces.Services;

namespace RecipeMeal.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AiFoodRecommendationController : ControllerBase
	{
		private readonly IFoodRecommendationService _foodRecommendationService;

		public AiFoodRecommendationController(IFoodRecommendationService foodRecommendationService)
		{
			_foodRecommendationService = foodRecommendationService;
		}

		[HttpPost("recommend")]
		[AllowAnonymous]
		public async Task<IActionResult> GetFoodRecommendation([FromBody] string userPrompt)
		{
			try
			{
				var recommendations = await _foodRecommendationService.GetFoodRecommendationsAsync(userPrompt);
				return Ok(new { recommendations });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}
	}
}

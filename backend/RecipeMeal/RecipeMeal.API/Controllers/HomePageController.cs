using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeMeal.Core.Interfaces.Services;

namespace RecipeMeal.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class HomePageController : ControllerBase
	{
		private readonly IHomePageService _homePageService;

		public HomePageController(IHomePageService homePageService)
		{
			_homePageService = homePageService;
		}

		[HttpGet("current")]
		[AllowAnonymous]
		public async Task<IActionResult> GetCurrentMealPlan([FromQuery] DateTime? date)
		{
			try
			{
				var currentDate = date ?? DateTime.Today;
				var response = await _homePageService.GetCurrentMealPlanAsync(currentDate);
				return Ok(response);
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

		[HttpGet("range")]
		[AllowAnonymous]
		public async Task<IActionResult> GetMealPlansByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
		{
			try
			{
				var response = await _homePageService.GetMealPlansByDateRangeAsync(startDate, endDate);
				return Ok(response);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}
	}
}

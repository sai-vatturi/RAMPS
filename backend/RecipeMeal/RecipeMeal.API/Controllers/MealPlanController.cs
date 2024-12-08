using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeMeal.Core.DTOs.MealPlan;
using RecipeMeal.Core.Interfaces.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RecipeMeal.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class MealPlanController : ControllerBase
	{
		private readonly IMealPlanService _mealPlanService;

		public MealPlanController(IMealPlanService mealPlanService)
		{
			_mealPlanService = mealPlanService;
		}

		[HttpPost]
		[Authorize(Roles = "MealPlanner,Admin")]
		public async Task<IActionResult> CreateMealPlan([FromBody] CreateMealPlanDto dto)
		{
			try
			{
				var createdBy = User.Identity?.Name;
				var mealPlan = await _mealPlanService.CreateMealPlanAsync(dto, createdBy);
				return Ok(mealPlan);
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

		[HttpPut("{id}")]
		[Authorize(Roles = "MealPlanner,Admin")]
		public async Task<IActionResult> UpdateMealPlan(int id, [FromBody] CreateMealPlanDto dto)
		{
			try
			{
				var updatedBy = User.Identity?.Name;
				var mealPlan = await _mealPlanService.UpdateMealPlanAsync(id, dto, updatedBy);
				return Ok(mealPlan);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(new { message = ex.Message });
			}
			catch (UnauthorizedAccessException ex)
			{
				return Forbid(ex.Message);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> GetMealPlans()
		{
			var mealPlans = await _mealPlanService.GetMealPlansAsync();
			return Ok(mealPlans);
		}


		[HttpGet("{id}")]
		[Authorize]
		public async Task<IActionResult> GetMealPlanById(int id)
		{
			try
			{
				var username = User.Identity?.Name;
				var isAdmin = User.IsInRole("Admin");
				var mealPlan = await _mealPlanService.GetMealPlanByIdAsync(id, username, isAdmin);
				return Ok(mealPlan);
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

		[HttpDelete("{id}")]
		[Authorize(Roles = "MealPlanner,Admin")]
		public async Task<IActionResult> DeleteMealPlan(int id)
		{
			try
			{
				var deletedBy = User.Identity?.Name;
				var isAdmin = User.IsInRole("Admin");
				var message = await _mealPlanService.DeleteMealPlanAsync(id, deletedBy, isAdmin);
				return Ok(new { message });
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

		[HttpPatch("{id}")]
		[Authorize(Roles = "MealPlanner,Admin")]
		public async Task<IActionResult> PatchMealPlan(int id, [FromBody] PatchMealPlanDto dto)
		{
			try
			{
				var updatedBy = User.Identity?.Name;
				var isAdmin = User.IsInRole("Admin");
				var mealPlan = await _mealPlanService.PatchMealPlanAsync(id, dto, updatedBy, isAdmin);
				return Ok(mealPlan);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(new { message = ex.Message });
			}
			catch (UnauthorizedAccessException ex)
			{
				return Forbid(ex.Message);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}
	}
}

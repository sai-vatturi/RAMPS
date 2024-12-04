using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeMeal.Core.DTOs.MealPlan;
using RecipeMeal.Core.Entities;
using RecipeMeal.Infrastructure.Data;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeMeal.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class MealPlanController : ControllerBase
	{
		private readonly RecipeMealDbContext _dbContext;

		public MealPlanController(RecipeMealDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		// Create a new meal plan
		[HttpPost]
		[Authorize(Roles = "MealPlanner,Admin")]
		public async Task<IActionResult> CreateMealPlan([FromBody] CreateMealPlanDto dto)
		{
			var mealPlan = new MealPlan
			{
				Name = dto.Name,
				CreatedBy = HttpContext.User.Identity.Name,
				StartDate = dto.StartDate,
				EndDate = dto.EndDate,
				Recipes = dto.Recipes.Select(r => new MealPlanRecipe
				{
					RecipeId = r.RecipeId,
					MealTime = r.MealTime
				}).ToList()
			};

			_dbContext.MealPlans.Add(mealPlan);
			await _dbContext.SaveChangesAsync();

			return Ok(mealPlan);
		}

		// Update a meal plan
		[HttpPut("{id}")]
		[Authorize(Roles = "MealPlanner,Admin")]
		public async Task<IActionResult> UpdateMealPlan(int id, [FromBody] CreateMealPlanDto dto)
		{
			var mealPlan = await _dbContext.MealPlans.Include(mp => mp.Recipes).FirstOrDefaultAsync(mp => mp.MealPlanId == id);
			if (mealPlan == null)
				return NotFound("Meal Plan not found.");

			if (mealPlan.CreatedBy != HttpContext.User.Identity.Name && !User.IsInRole("Admin"))
				return Forbid("You are not authorized to update this meal plan.");

			mealPlan.Name = dto.Name;
			mealPlan.StartDate = dto.StartDate;
			mealPlan.EndDate = dto.EndDate;
			mealPlan.Recipes = dto.Recipes.Select(r => new MealPlanRecipe
			{
				RecipeId = r.RecipeId,
				MealTime = r.MealTime
			}).ToList();

			_dbContext.MealPlans.Update(mealPlan);
			await _dbContext.SaveChangesAsync();

			return Ok(mealPlan);
		}

		// Get all meal plans for the logged-in user
		[HttpGet]
		[Authorize(Roles = "User,Admin,MealPlanner")]
		public IActionResult GetMealPlans()
		{
			var username = HttpContext.User.Identity.Name;
			var mealPlans = _dbContext.MealPlans
				.Where(mp => mp.CreatedBy == username || User.IsInRole("Admin"))
				.Select(mp => new
				{
					mp.MealPlanId,
					mp.Name,
					mp.StartDate,
					mp.EndDate,
					Recipes = mp.Recipes.Select(r => new
					{
						r.RecipeId,
						r.Recipe.Title,
						r.MealTime
					})
				}).ToList();

			return Ok(mealPlans);
		}

		// Get a specific meal plan by ID
		[HttpGet("{id}")]
		[Authorize(Roles = "User,Admin,MealPlanner")]
		public async Task<IActionResult> GetMealPlanById(int id)
		{
			var mealPlan = await _dbContext.MealPlans
				.Include(mp => mp.Recipes)
				.ThenInclude(r => r.Recipe)
				.FirstOrDefaultAsync(mp => mp.MealPlanId == id);

			if (mealPlan == null)
				return NotFound("Meal Plan not found.");

			if (mealPlan.CreatedBy != HttpContext.User.Identity.Name && !User.IsInRole("Admin"))
				return Forbid("You are not authorized to view this meal plan.");

			var result = new
			{
				mealPlan.MealPlanId,
				mealPlan.Name,
				mealPlan.StartDate,
				mealPlan.EndDate,
				Recipes = mealPlan.Recipes.Select(r => new
				{
					r.RecipeId,
					r.Recipe.Title,
					r.MealTime
				})
			};

			return Ok(result);
		}

		// Delete a meal plan
		[HttpDelete("{id}")]
		[Authorize(Roles = "MealPlanner,Admin")]
		public async Task<IActionResult> DeleteMealPlan(int id)
		{
			var mealPlan = await _dbContext.MealPlans.FindAsync(id);
			if (mealPlan == null)
				return NotFound("Meal Plan not found.");

			if (mealPlan.CreatedBy != HttpContext.User.Identity.Name && !User.IsInRole("Admin"))
				return Forbid("You are not authorized to delete this meal plan.");

			_dbContext.MealPlans.Remove(mealPlan);
			await _dbContext.SaveChangesAsync();

			return Ok("Meal Plan deleted successfully.");
		}
	}
}

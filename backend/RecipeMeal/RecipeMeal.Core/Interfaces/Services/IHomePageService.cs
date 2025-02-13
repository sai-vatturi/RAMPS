using RecipeMeal.Core.DTOs.HomePage;

namespace RecipeMeal.Core.Interfaces.Services
{
	public interface IHomePageService
	{
		Task<HomePageResponseDto> GetCurrentMealPlanAsync(DateTime date);
		Task<List<HomePageResponseDto>> GetMealPlansByDateRangeAsync(DateTime startDate, DateTime endDate);
	}
}

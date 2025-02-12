using System.Threading.Tasks;

namespace RecipeMeal.Core.Interfaces.Services
{
	public interface IFoodRecommendationService
	{
		Task<string> GetFoodRecommendationsAsync(string userPrompt);
	}
}

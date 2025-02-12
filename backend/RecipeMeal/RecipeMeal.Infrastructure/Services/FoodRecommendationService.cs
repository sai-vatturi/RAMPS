using Microsoft.Extensions.Configuration;
using RecipeMeal.Core.Interfaces.Services;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RecipeMeal.Infrastructure.Services
{
	public class FoodRecommendationService : IFoodRecommendationService
	{
		private readonly IRecipeService _recipeService;
		private readonly IConfiguration _configuration;
		private readonly HttpClient _httpClient;

		public FoodRecommendationService(
			IRecipeService recipeService,
			IConfiguration configuration,
			HttpClient httpClient)
		{
			_recipeService = recipeService;
			_configuration = configuration;
			_httpClient = httpClient;
		}

		public async Task<string> GetFoodRecommendationsAsync(string userPrompt)
		{
			try
			{
				// Get all recipes from the database
				var recipes = await _recipeService.GetAllRecipesAsync(0, 0); // Example values for pageNumber and pageSize

				// Build the AI prompt
				var promptBuilder = new StringBuilder();
				promptBuilder.AppendLine($"Based on the user condition: '{userPrompt}', analyze the following recipes and recommend the most suitable ones. For each recommended recipe, provide a specific reason why it's beneficial for the user's condition.\n\nAvailable recipes:\n");

				foreach (var recipe in recipes)
				{
					var recipeObj = (dynamic)recipe;
					promptBuilder.AppendLine($"- {recipeObj.Title}");
					if (!string.IsNullOrEmpty(recipeObj.Description?.ToString()))
					{
						promptBuilder.AppendLine($"  Description: {recipeObj.Description}");
					}
					if (!string.IsNullOrEmpty(recipeObj.Ingredients?.ToString()))
					{
						promptBuilder.AppendLine($"  Ingredients: {recipeObj.Ingredients}");
					}
				}

				promptBuilder.AppendLine("\nPlease provide recommendations in this format:");
				promptBuilder.AppendLine("Recipe Name - Specific reason why it's beneficial for the condition");

				// Call Gemini API
				var recommendations = await CallGeminiApiAsync(promptBuilder.ToString());
				return recommendations;
			}
			catch (Exception ex)
			{
				throw new Exception($"Error getting food recommendations: {ex.Message}", ex);
			}
		}

		private async Task<string> CallGeminiApiAsync(string prompt)
		{
			var geminiApiKey = _configuration["GeminiApi:Key"];
			if (string.IsNullOrEmpty(geminiApiKey))
			{
				throw new Exception("Gemini API key not configured");
			}

			var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent?key={geminiApiKey}";

			var requestBody = new
			{
				contents = new[]
				{
					new
					{
						parts = new[]
						{
							new { text = prompt }
						}
					}
				}
			};

			var response = await _httpClient.PostAsJsonAsync(url, requestBody);

			if (!response.IsSuccessStatusCode)
			{
				var errorContent = await response.Content.ReadAsStringAsync();
				throw new Exception($"Gemini API error: {errorContent}");
			}

			var geminiResponse = await response.Content.ReadFromJsonAsync<GeminiResponse>();
			var recommendations = geminiResponse?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text;

			if (string.IsNullOrEmpty(recommendations))
			{
				throw new Exception("No recommendations received from AI");
			}

			return recommendations;
		}

		private class GeminiResponse
		{
			public List<Candidate> Candidates { get; set; }
		}

		private class Candidate
		{
			public Content Content { get; set; }
		}

		private class Content
		{
			public List<Part> Parts { get; set; }
		}

		private class Part
		{
			public string Text { get; set; }
		}
	}
}

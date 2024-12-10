using RecipeMeal.Core.DTOs;
using RecipeMeal.Core.DTOs.BMR;
using RecipeMeal.Core.Interfaces.Services;
using System;
using System.Threading.Tasks;

namespace RecipeMeal.Infrastructure.Services
{
	public class BMRCalculatorService : IBMRCalculatorService
	{

		public Task<double> CalculateBMRAsync(BMRCalculatorDto dto)
		{
			if (dto == null)
				throw new ArgumentNullException(nameof(dto));

			double bmr;

			// Harris-Benedict equation for BMR calculation
			if (dto.Gender.ToLower() == "male")
			{
				// BMR for men: 88.362 + (13.397 * weight in kg) + (4.799 * height in cm) - (5.677 * age in years)
				bmr = 88.362 + (13.397 * dto.Weight) + (4.799 * dto.Height) - (5.677 * dto.Age);
			}
			else if (dto.Gender.ToLower() == "female")
			{
				// BMR for women: 447.593 + (9.247 * weight in kg) + (3.098 * height in cm) - (4.330 * age in years)
				bmr = 447.593 + (9.247 * dto.Weight) + (3.098 * dto.Height) - (4.330 * dto.Age);
			}
			else
			{
				throw new Exception("Invalid gender provided. Gender must be either 'male' or 'female'.");
			}

			// Adjust BMR based on activity level (use multiplication factors)
			double activityFactor = dto.ActivityLevel.ToLower() switch
			{
				"sedentary" => 1.2,
				"light" => 1.375,
				"moderate" => 1.55,
				"active" => 1.725,
				"veryactive" => 1.9,
				_ => throw new Exception("Invalid activity level provided. Valid values are: Sedentary, Light, Moderate, Active, VeryActive.")
			};

			bmr *= activityFactor;

			return Task.FromResult(bmr);
		}
	}
}

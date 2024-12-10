namespace RecipeMeal.Core.DTOs.BMR
{
	public class BMRCalculatorDto
	{
		public double Weight { get; set; } // Weight in kilograms
		public double Height { get; set; } // Height in centimeters
		public int Age { get; set; } // Age in years
		public string Gender { get; set; } // Gender: Male or Female
		public string ActivityLevel { get; set; } // Activity Level: Sedentary, Light, Moderate, Active, VeryActive
	}
}

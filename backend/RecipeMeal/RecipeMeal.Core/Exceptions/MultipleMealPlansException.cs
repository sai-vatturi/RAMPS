namespace RecipeMeal.Core.Exceptions
{
	public class MultipleMealPlansException : Exception
	{
		public MultipleMealPlansException(DateTime date)
			: base($"Multiple meal plans found for date {date:yyyy-MM-dd}. Please check meal plan configurations.")
		{
		}
	}
}

namespace RecipeMeal.Core.Interfaces
{
	public interface IEmailService
	{
		Task SendAsync(string to, string subject, string body);
	}
}

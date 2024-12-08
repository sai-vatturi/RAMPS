using System.ComponentModel.DataAnnotations;

namespace RecipeMeal.Core.DTOs.Auth
{
	public class SignupDto
	{

		[Required]
		[StringLength(50)]
		public string FirstName { get; set; }

		[Required]
		[StringLength(50)]

		public string LastName { get; set; }
		[Required]
		[StringLength(50, MinimumLength = 3)]
		public string Username { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[StringLength(100, MinimumLength = 6)]
		public string Password { get; set; }

		[Phone]
		public string PhoneNumber { get; set; }

		[Required]
		public string Role { get; set; }

	}
}

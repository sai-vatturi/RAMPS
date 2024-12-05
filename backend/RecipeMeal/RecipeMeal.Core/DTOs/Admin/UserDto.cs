namespace RecipeMeal.Core.DTOs
{
	using System.ComponentModel.DataAnnotations;

	public class UserDto
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

		[Required] // Ensure phone number is mandatory
		[Phone]
		public string PhoneNumber { get; set; }

		[Required]
		public string Role { get; set; } // Role as a string

		public bool IsApproved { get; set; }
	}


}

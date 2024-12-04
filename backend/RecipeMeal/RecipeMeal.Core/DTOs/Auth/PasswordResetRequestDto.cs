using System.ComponentModel.DataAnnotations;

namespace RecipeMeal.Core.DTOs.Auth
{
	public class PasswordResetRequestDto
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
	}

}

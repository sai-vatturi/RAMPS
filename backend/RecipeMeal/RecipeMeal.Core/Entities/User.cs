using System.ComponentModel.DataAnnotations;
using RecipeMeal.Core.Enums;

namespace RecipeMeal.Core.Entities
{
	public class User
	{
		public int UserId { get; set; }

		[Required]
		[StringLength(50, MinimumLength = 3)]
		public string Username { get; set; }

		[Required]
		[StringLength(50)]
		public string FirstName { get; set; }

		[Required]
		[StringLength(50)]
		public string LastName { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		public string PasswordHash { get; set; }

		[Required]
		[Phone]
		public string PhoneNumber { get; set; }

		[Required]
		public Role Role { get; set; }

		public bool IsApproved { get; set; }
		public bool IsActive { get; set; }
		public bool IsEmailVerified { get; set; }
		public string? EmailVerificationToken { get; set; }
		public string? PasswordResetToken { get; set; }
		public DateTime? PasswordResetTokenExpiry { get; set; }

	}
}

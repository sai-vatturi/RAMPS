using System.ComponentModel.DataAnnotations;

namespace RecipeMeal.Core.DTOs.Admin
{
	public class UpdateRoleDto
	{
		[Required]
		public string Username { get; set; }

		[Required]
		public string NewRole { get; set; }
	}

}

namespace RecipeMeal.Core.DTOs.Admin
{
	public class ApproveUserDto
	{
		public string Username { get; set; } = string.Empty;
		public bool IsApproved { get; set; } = true;
	}
}

namespace RecipeMeal.Core.DTOs.Admin
{
	public class ApproveUserDto
	{
    	public string Username { get; set; } = string.Empty; // Identify the user by username
    	public bool IsApproved { get; set; } = true; // Default to approving the user
	}
}

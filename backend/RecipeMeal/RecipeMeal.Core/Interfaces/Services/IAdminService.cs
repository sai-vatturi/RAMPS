using RecipeMeal.Core.DTOs;
using RecipeMeal.Core.DTOs.Admin;

namespace RecipeMeal.Core.Interfaces.Services
{
	public interface IAdminService
	{
		Task<object> GetAllUsersAsync();
		Task<string> AddUserAsync(UserDto userDto);
		Task<string> UpdateRoleAsync(UpdateRoleDto dto);
		Task<string> ApproveUserAsync(ApproveUserDto dto);
		Task<string> DeleteUserAsync(string username);
	}
}

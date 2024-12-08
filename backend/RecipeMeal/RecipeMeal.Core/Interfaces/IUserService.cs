using RecipeMeal.Core.DTOs;
using RecipeMeal.Core.DTOs.Admin;
using RecipeMeal.Core.DTOs.Auth;
using RecipeMeal.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeMeal.Core.Interfaces
{
	public interface IUserService
	{
		Task<List<User>> GetAllUsersAsync();
		Task AddUserAsync(SignupDto userDto);
		Task UpdateRoleAsync(UpdateRoleDto dto);
		Task ApproveUserAsync(ApproveUserDto dto);
		Task DeleteUserAsync(string username);
		Task<User> LoginAsync(LoginDto dto);
		Task RequestPasswordResetAsync(PasswordResetRequestDto dto);
		Task ResetPasswordAsync(ResetPasswordDto dto);
		Task SignupAsync(SignupDto dto);
		Task VerifyEmailAsync(string token);
	}
}

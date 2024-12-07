using RecipeMeal.Core.DTOs.Auth;
using System.Threading.Tasks;

namespace RecipeMeal.Core.Interfaces.Services
{
	public interface IAuthService
	{
		Task<string> SignupAsync(SignupDto dto);
		Task<string> VerifyEmailAsync(string token);
		Task<string> LoginAsync(LoginDto dto);
		Task RequestPasswordResetAsync(PasswordResetRequestDto dto);
		Task ResetPasswordAsync(ResetPasswordDto dto);
		Task<object> GetCurrentUserAsync(string username);
	}
}

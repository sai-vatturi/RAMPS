using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeMeal.Core.DTOs.Auth;
using RecipeMeal.Core.Interfaces.Services;
using System.Threading.Tasks;

namespace RecipeMeal.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;

		public AuthController(IAuthService authService)
		{
			_authService = authService;
		}

		[HttpPost("signup")]
		public async Task<IActionResult> Signup([FromBody] SignupDto dto)
		{
			try
			{
				var result = await _authService.SignupAsync(dto);
				return Ok(result);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpGet("verify-email")]
		public async Task<IActionResult> VerifyEmail(string token)
		{
			try
			{
				var result = await _authService.VerifyEmailAsync(token);
				return Ok(result);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginDto dto)
		{
			try
			{
				var token = await _authService.LoginAsync(dto);
				return Ok(new { token });
			}
			catch (UnauthorizedAccessException ex) // Specific exception for authentication errors
			{
				return Unauthorized(new { message = ex.Message });
			}
			catch (Exception ex) // Generic exception for other errors
			{
				return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
			}
		}


		[HttpGet("me")]
		[Authorize]
		public async Task<IActionResult> GetCurrentUser()
		{
			try
			{
				var username = User.Identity?.Name;
				var userDetails = await _authService.GetCurrentUserAsync(username);
				return Ok(userDetails);
			}
			catch (UnauthorizedAccessException ex)
			{
				return Unauthorized(new { message = ex.Message });
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(new { message = ex.Message });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = "An unexpected error occurred.", error = ex.Message });
			}
		}


		[HttpPost("request-password-reset")]
		public async Task<IActionResult> RequestPasswordReset([FromBody] PasswordResetRequestDto dto)
		{
			try
			{
				await _authService.RequestPasswordResetAsync(dto);
				return Ok(new { message = "Password reset link has been sent to your email." });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpPost("reset-password")]
		public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
		{
			try
			{
				await _authService.ResetPasswordAsync(dto);
				return Ok(new { message = "Password has been reset successfully." });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}


	}
}

using Microsoft.AspNetCore.Mvc;
using RecipeMeal.Core.DTOs.Auth;
using RecipeMeal.Core.Entities;
using RecipeMeal.Core.Services;
using RecipeMeal.Infrastructure.Data;
using System.Linq;
using BCrypt.Net;
using RecipeMeal.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using RecipeMeal.Core.Interfaces;
using RecipeMeal.Infrastructure.Validators;

namespace RecipeMeal.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController : ControllerBase
	{
		private readonly RecipeMealDbContext _dbContext;
		private readonly JwtService _jwtService;
		private readonly IEmailService _emailService;
		private readonly UserValidationService _userValidationService;

		public AuthController(RecipeMealDbContext dbContext, JwtService jwtService, IEmailService emailService, UserValidationService userValidationService)
		{
			_dbContext = dbContext;
			_jwtService = jwtService;
			_emailService = emailService;
			_userValidationService = userValidationService;
		}

		[HttpPost("signup")]
		public async Task<IActionResult> Signup([FromBody] SignupDto dto)
		{
			if (await _userValidationService.EmailExistsAsync(dto.Email))
				return BadRequest("Email already exists.");

			if (await _userValidationService.UsernameExistsAsync(dto.Username))
				return BadRequest("Username already exists.");

			if (!Enum.TryParse<Role>(dto.Role, true, out var role))
				return BadRequest("Invalid role.");

			var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

			var user = new User
			{
				FirstName = dto.FirstName, // Assign FirstName
				LastName = dto.LastName,   // Assign LastName
				Username = dto.Username,
				Email = dto.Email,
				PasswordHash = passwordHash,
				PhoneNumber = dto.PhoneNumber,
				Role = role,
				IsApproved = role == Role.User,
				IsActive = true,
				IsEmailVerified = false,
				EmailVerificationToken = Guid.NewGuid().ToString()
			};

			_dbContext.Users.Add(user);
			await _dbContext.SaveChangesAsync();

			// Send email with verification link
			string verificationLink = $"https://yourfrontend.com/verify-email?token={user.EmailVerificationToken}";
			await _emailService.SendAsync(user.Email, "Verify Your Email", $"Click here to verify your email: {verificationLink}");

			return Ok("User registered successfully. Please check your email to verify your account.");
		}


		[HttpGet("verify-email")]
		public async Task<IActionResult> VerifyEmail(string token)
		{
			var user = _dbContext.Users.FirstOrDefault(u => u.EmailVerificationToken == token);

			if (user == null)
				return BadRequest("Invalid token.");

			user.IsEmailVerified = true;
			user.EmailVerificationToken = null;
			_dbContext.Users.Update(user);
			await _dbContext.SaveChangesAsync();

			return Ok("Email verified successfully.");
		}


		[HttpPost("login")]
		public IActionResult Login([FromBody] LoginDto dto)
		{
			var user = _dbContext.Users.FirstOrDefault(u => u.Username == dto.Username);

			if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
				return Unauthorized("Invalid username or password.");

			if (!user.IsActive)
				return Unauthorized("Account is deactivated.");

			if (!user.IsApproved)
				return Unauthorized("Account is pending approval.");

			if (!user.IsEmailVerified)
				return Unauthorized("Please verify your email before logging in.");

			var token = _jwtService.GenerateJwtToken(user); // Use JwtService here
			return Ok(new { token });
		}

		[HttpGet("me")]
		[Authorize]
		public IActionResult GetCurrentUser()
		{
			var username = User.Identity?.Name;

			if (username == null)
				return Unauthorized("Invalid token.");

			var user = _dbContext.Users.FirstOrDefault(u => u.Username == username);

			if (user == null)
				return NotFound("User not found.");

			return Ok(new
			{
				user.FirstName,
				user.Username,
				Role = user.Role.ToString()
			});
		}


		[HttpPost("request-password-reset")]
		public async Task<IActionResult> RequestPasswordReset([FromBody] PasswordResetRequestDto dto)
		{
			if (string.IsNullOrEmpty(dto.Email))
			{
				return BadRequest("The email field is required.");
			}

			var user = _dbContext.Users.FirstOrDefault(u => u.Email == dto.Email);

			if (user == null)
			{
				return NotFound("User with this email does not exist.");
			}

			user.PasswordResetToken = Guid.NewGuid().ToString();
			user.PasswordResetTokenExpiry = DateTime.UtcNow.AddHours(1);
			_dbContext.Users.Update(user);
			await _dbContext.SaveChangesAsync();

			string resetLink = $"https://yourfrontend.com/reset-password?token={user.PasswordResetToken}";
			await _emailService.SendAsync(user.Email, "Reset Your Password", $"Click here to reset your password: {resetLink}");

			return Ok("Password reset link has been sent to your email.");
		}



		[HttpPost("reset-password")]
		public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
		{
			var user = _dbContext.Users.FirstOrDefault(u => u.PasswordResetToken == dto.Token);

			if (user == null || user.PasswordResetTokenExpiry < DateTime.UtcNow)
				return BadRequest("Invalid or expired token.");

			user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
			user.PasswordResetToken = null;
			user.PasswordResetTokenExpiry = null;
			_dbContext.Users.Update(user);
			await _dbContext.SaveChangesAsync();

			return Ok("Password has been reset successfully.");
		}

		[HttpPost("send-test-email")]
		public async Task<IActionResult> SendTestEmail(string email)
		{
			await _emailService.SendAsync(email, "Test Email", "<p>This is a test email from RecipeMeal API.</p>");
			return Ok("Test email sent.");
		}

	}
}

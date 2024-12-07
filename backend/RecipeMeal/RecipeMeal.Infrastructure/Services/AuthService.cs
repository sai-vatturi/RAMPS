using RecipeMeal.Core.DTOs.Auth;
using RecipeMeal.Core.Entities;
using RecipeMeal.Core.Enums;
using RecipeMeal.Core.Interfaces.Services;
using RecipeMeal.Infrastructure.Data;
using RecipeMeal.Infrastructure.Validators;
using BCrypt.Net;
using System;
using System.Linq;
using System.Threading.Tasks;
using RecipeMeal.Core.Interfaces;
using RecipeMeal.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace RecipeMeal.Infrastructure.Services
{
	public class AuthService : IAuthService
	{
		private readonly RecipeMealDbContext _dbContext;
		private readonly IEmailService _emailService;
		private readonly JwtService _jwtService;
		private readonly UserValidationService _userValidationService;

		public AuthService(
			RecipeMealDbContext dbContext,
			IEmailService emailService,
			JwtService jwtService,
			UserValidationService userValidationService)
		{
			_dbContext = dbContext;
			_emailService = emailService;
			_jwtService = jwtService;
			_userValidationService = userValidationService;
		}

		public async Task<string> SignupAsync(SignupDto dto)
		{
			if (await _userValidationService.EmailExistsAsync(dto.Email))
				return "Email already exists.";

			if (await _userValidationService.UsernameExistsAsync(dto.Username))
				return "Username already exists.";

			if (!Enum.TryParse<Role>(dto.Role, true, out var role))
				return "Invalid role.";

			var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

			var user = new User
			{
				FirstName = dto.FirstName,
				LastName = dto.LastName,
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

			string verificationLink = $"https://yourfrontend.com/verify-email?token={user.EmailVerificationToken}";
			await _emailService.SendAsync(user.Email, "Verify Your Email", $"Click here to verify your email: {verificationLink}");

			return "User registered successfully. Please check your email to verify your account.";
		}

		public async Task<string> VerifyEmailAsync(string token)
		{
			var user = _dbContext.Users.FirstOrDefault(u => u.EmailVerificationToken == token);

			if (user == null)
				return "Invalid token.";

			user.IsEmailVerified = true;
			user.EmailVerificationToken = null;
			_dbContext.Users.Update(user);
			await _dbContext.SaveChangesAsync();

			return "Email verified successfully.";
		}

		public async Task<string> LoginAsync(LoginDto dto)
		{
			var user = _dbContext.Users.FirstOrDefault(u => u.Username == dto.Username);

			if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
				return "Invalid username or password.";

			if (!user.IsActive)
				return "Account is deactivated.";

			if (!user.IsApproved)
				return "Account is pending approval.";

			if (!user.IsEmailVerified)
				return "Please verify your email before logging in.";

			return _jwtService.GenerateJwtToken(user);
		}

		public async Task RequestPasswordResetAsync(PasswordResetRequestDto dto)
		{
			var user = _dbContext.Users.FirstOrDefault(u => u.Email == dto.Email);

			if (user == null)
				throw new Exception("User with this email does not exist.");

			user.PasswordResetToken = Guid.NewGuid().ToString();
			user.PasswordResetTokenExpiry = DateTime.UtcNow.AddHours(1);
			_dbContext.Users.Update(user);
			await _dbContext.SaveChangesAsync();

			string resetLink = $"https://yourfrontend.com/reset-password?token={user.PasswordResetToken}";
			await _emailService.SendAsync(user.Email, "Reset Your Password", $"Click here to reset your password: {resetLink}");
		}

		public async Task ResetPasswordAsync(ResetPasswordDto dto)
		{
			var user = _dbContext.Users.FirstOrDefault(u => u.PasswordResetToken == dto.Token);

			if (user == null || user.PasswordResetTokenExpiry < DateTime.UtcNow)
				throw new Exception("Invalid or expired token.");

			user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
			user.PasswordResetToken = null;
			user.PasswordResetTokenExpiry = null;
			_dbContext.Users.Update(user);
			await _dbContext.SaveChangesAsync();
		}

		public async Task<object> GetCurrentUserAsync(string username)
		{
			if (string.IsNullOrEmpty(username))
				throw new UnauthorizedAccessException("Invalid token.");

			var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);

			if (user == null)
				throw new KeyNotFoundException("User not found.");

			return new
			{
				user.FirstName,
				user.Username,
				Role = user.Role.ToString()
			};
		}
	}
}

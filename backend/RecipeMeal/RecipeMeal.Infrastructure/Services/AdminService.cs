using RecipeMeal.Core.DTOs;
using RecipeMeal.Core.DTOs.Admin;
using RecipeMeal.Core.Entities;
using RecipeMeal.Core.Enums;
using RecipeMeal.Core.Interfaces.Services;
using RecipeMeal.Infrastructure.Data;
using RecipeMeal.Infrastructure.Validators;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using RecipeMeal.Core.Interfaces;

namespace RecipeMeal.Infrastructure.Services
{
	public class AdminService : IAdminService
	{
		private readonly RecipeMealDbContext _dbContext;
		private readonly IEmailService _emailService;
		private readonly UserValidationService _userValidationService;

		public AdminService(
			RecipeMealDbContext dbContext,
			IEmailService emailService,
			UserValidationService userValidationService)
		{
			_dbContext = dbContext;
			_emailService = emailService;
			_userValidationService = userValidationService;
		}

		public async Task<object> GetAllUsersAsync()
		{
			var users = await _dbContext.Users
				.Select(u => new
				{
					u.FirstName,
					u.LastName,
					u.Username,
					u.Email,
					u.Role,
					u.IsApproved
				}).ToListAsync();

			return users;
		}

		public async Task<string> AddUserAsync(UserDto userDto)
		{
			if (string.IsNullOrWhiteSpace(userDto.PhoneNumber))
				throw new Exception("Phone number is required.");

			if (await _userValidationService.EmailExistsAsync(userDto.Email))
				throw new Exception("Email already exists.");

			if (await _userValidationService.UsernameExistsAsync(userDto.Username))
				throw new Exception("Username already exists.");

			if (!Enum.TryParse<Role>(userDto.Role, true, out var parsedRole))
				throw new Exception($"Invalid role: {userDto.Role}");

			var user = new User
			{
				FirstName = userDto.FirstName,
				LastName = userDto.LastName,
				Username = userDto.Username,
				Email = userDto.Email,
				PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
				PhoneNumber = userDto.PhoneNumber,
				Role = (Role)parsedRole,
				IsApproved = userDto.IsApproved,
			};

			_dbContext.Users.Add(user);
			await _dbContext.SaveChangesAsync();

			return "User added successfully.";
		}

		public async Task<string> UpdateRoleAsync(UpdateRoleDto dto)
		{
			var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
			if (user == null)
				throw new Exception("User not found.");

			if (!Enum.TryParse<Role>(dto.NewRole, true, out var parsedRole))
				throw new Exception($"Invalid role: {dto.NewRole}");

			user.Role = (Role)parsedRole;
			_dbContext.Users.Update(user);
			await _dbContext.SaveChangesAsync();

			return $"Role updated successfully for user {dto.Username}.";
		}

		public async Task<string> ApproveUserAsync(ApproveUserDto dto)
		{
			var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);

			if (user == null)
				throw new Exception("User not found.");

			user.IsApproved = dto.IsApproved;
			_dbContext.Users.Update(user);
			await _dbContext.SaveChangesAsync();

			if (dto.IsApproved)
			{
				await _emailService.SendAsync(user.Email, "Account Approved",
					$"Dear {user.FirstName},\n\nYour account has been approved by the admin. You can now log in and use the system.\n\nBest regards,\nRecipeMeal Team");
			}

			return $"User {user.Username} has been {(dto.IsApproved ? "approved" : "disapproved")}.";
		}

		public async Task<string> DeleteUserAsync(string username)
		{
			var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
			if (user == null)
				throw new Exception("User not found.");

			_dbContext.Users.Remove(user);
			await _dbContext.SaveChangesAsync();

			return $"User {username} deleted successfully.";
		}
	}
}

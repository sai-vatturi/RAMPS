using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeMeal.Core.DTOs.Auth;
using RecipeMeal.Core.Entities;
using RecipeMeal.Core.Interfaces;
using RecipeMeal.Infrastructure.Data;
using System.Linq;
using System.Threading.Tasks;
using RecipeMeal.Core.DTOs.Admin;
using RecipeMeal.Core.DTOs;
using RecipeMeal.Infrastructure.Validators;

namespace RecipeMeal.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Authorize(Roles = "Admin")] // Restrict all endpoints to Admin role
	public class AdminController : ControllerBase
	{
		private readonly RecipeMealDbContext _dbContext;
		private readonly IEmailService _emailService;

		private readonly UserValidationService _userValidationService;

		public AdminController(RecipeMealDbContext dbContext, IEmailService emailService, UserValidationService userValidationService)
		{
			_dbContext = dbContext;
			_emailService = emailService;
			_userValidationService = userValidationService;
		}

		// Get all users
		[HttpGet("users")]
		[Authorize(Roles = "Admin")] // Only Admins can see all users
		public IActionResult GetAllUsers()
		{
			var users = _dbContext.Users
				.Select(u => new
				{
					u.FirstName,
					u.LastName,
					u.Username,
					u.Email,
					u.Role,
					u.IsApproved
				})
				.ToList();

			return Ok(users);
		}

		[HttpPost("add-user")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> AddUser([FromBody] UserDto userDto)
		{
			// Validate the model state
			if (!ModelState.IsValid)
			{
				var errors = ModelState.Values
					.SelectMany(v => v.Errors)
					.Select(e => e.ErrorMessage)
					.ToList();

				return BadRequest(new
				{
					message = "Validation failed",
					errors
				});
			}

			// Validate PhoneNumber
			if (string.IsNullOrWhiteSpace(userDto.PhoneNumber))
			{
				return BadRequest(new { message = "Phone number is required." });
			}

			// Check if email already exists
			if (await _userValidationService.EmailExistsAsync(userDto.Email))
			{
				return BadRequest(new { message = "Email already exists." });
			}

			// Check if username already exists
			if (await _userValidationService.UsernameExistsAsync(userDto.Username))
			{
				return BadRequest(new { message = "Username already exists." });
			}

			// Parse and validate the role
			if (!Enum.TryParse(typeof(RecipeMeal.Core.Enums.Role), userDto.Role, true, out var parsedRole))
			{
				return BadRequest(new { message = $"Invalid role: {userDto.Role}" });
			}

			// Map DTO to User entity
			var user = new User
			{
				FirstName = userDto.FirstName,
				LastName = userDto.LastName,
				Username = userDto.Username,
				Email = userDto.Email,
				PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
				PhoneNumber = userDto.PhoneNumber, // Ensure PhoneNumber is set
				Role = (RecipeMeal.Core.Enums.Role)parsedRole,
				IsApproved = userDto.IsApproved,
			};

			try
			{
				// Save to database
				_dbContext.Users.Add(user);
				await _dbContext.SaveChangesAsync();

				return Ok(new { message = "User added successfully." });
			}
			catch (Exception ex)
			{
				// Log exception
				return StatusCode(500, new { message = "Internal server error.", error = ex.Message });
			}
		}



		[HttpPut("update-role")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleDto dto)
		{
			var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
			if (user == null)
				return NotFound(new { message = "User not found." });

			if (!Enum.TryParse(typeof(RecipeMeal.Core.Enums.Role), dto.NewRole, true, out var parsedRole))
				return BadRequest(new { message = $"Invalid role: {dto.NewRole}" });

			user.Role = (RecipeMeal.Core.Enums.Role)parsedRole;
			_dbContext.Users.Update(user);
			await _dbContext.SaveChangesAsync();

			return Ok(new { message = $"Role updated successfully for user {dto.Username}." });
		}



		[HttpPut("approve-user")]
		[Authorize(Roles = "Admin")] // Only Admins can approve users
		public async Task<IActionResult> ApproveUser([FromBody] ApproveUserDto dto)
		{
			var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);

			if (user == null)
				return NotFound("User not found.");

			// Update the approval status of the user
			user.IsApproved = dto.IsApproved;

			_dbContext.Users.Update(user);
			await _dbContext.SaveChangesAsync();

			// Send email notification to user after approval
			if (dto.IsApproved)
			{
				await _emailService.SendAsync(user.Email, "Account Approved",
					$"Dear {user.FirstName},\n\nYour account has been approved by the admin. You can now log in and use the system.\n\nBest regards,\nRecipeMeal Team");
			}

			return Ok($"User with username {user.Username} has been {(dto.IsApproved ? "approved" : "disapproved")}.");
		}

		[HttpDelete("delete-user/{username}")]
		[Authorize(Roles = "Admin")]
		public IActionResult DeleteUser(string username)
		{
			var user = _dbContext.Users.FirstOrDefault(u => u.Username == username);
			if (user == null)
				return NotFound("User not found.");

			_dbContext.Users.Remove(user);
			_dbContext.SaveChanges();

			return Ok(new { message = $"User {username} deleted successfully." });
		}



	}
}

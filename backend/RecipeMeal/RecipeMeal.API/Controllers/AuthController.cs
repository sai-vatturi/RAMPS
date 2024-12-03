using Microsoft.AspNetCore.Mvc;
using RecipeMeal.Core.DTOs.Auth;
using RecipeMeal.Core.Entities;
using RecipeMeal.Core.Services;
using RecipeMeal.Infrastructure.Data;
using System.Linq;
using BCrypt.Net;
using RecipeMeal.Core.Enums;
using Microsoft.AspNetCore.Authorization;


namespace RecipeMeal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly RecipeMealDbContext _dbContext;
        private readonly JwtService _jwtService;

        public AuthController(RecipeMealDbContext dbContext, JwtService jwtService)
        {
            _dbContext = dbContext;
            _jwtService = jwtService;
        }

        [HttpPost("signup")]
		public async Task<IActionResult> Signup([FromBody] SignupDto dto)
		{
			// Check if username or email already exists
			if (_dbContext.Users.Any(u => u.Username == dto.Username || u.Email == dto.Email))
				return BadRequest("Username or Email already exists.");

			// Validate role
			if (!Enum.TryParse<Role>(dto.Role, true, out var role))
				return BadRequest("Invalid role.");

			// Hash password
			var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

			// Create new user
			var user = new User
			{
				Username = dto.Username,
				Email = dto.Email,
				PasswordHash = passwordHash,
				PhoneNumber = dto.PhoneNumber,
				Role = role,
				IsApproved = role == Role.User, // Auto-approve only regular users
				IsActive = true,
				FirstName = dto.FirstName,
				LastName = dto.LastName
			};

			_dbContext.Users.Add(user);
			await _dbContext.SaveChangesAsync();

			return Ok("User registered successfully.");
		}

        // User Login
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

            // Generate JWT token using JwtService
            var token = _jwtService.GenerateJwtToken(user);
            return Ok(new { token });
        }

		[HttpGet("getallusers")]
		[Authorize(Roles = "Admin")] // Restrict access to Admin only
		public IActionResult GetAllUsers()
		{
			// Check if the authenticated user has Admin role
			var currentUser = HttpContext.User;
			if (!currentUser.IsInRole("Admin"))
			{
				return Forbid("You are not authorized to access this endpoint.");
			}

			// Retrieve all users from the database
			var users = _dbContext.Users
				.Select(u => new
				{
					u.FirstName,
					u.LastName,
					u.Username,
					u.Email,
					Role = u.Role.ToString(),
				})
				.ToList();

			return Ok(users);
		}

    }
}

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

namespace RecipeMeal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")] // Restrict all endpoints to Admin role
    public class AdminController : ControllerBase
    {
        private readonly RecipeMealDbContext _dbContext;
        private readonly IEmailService _emailService;

        public AdminController(RecipeMealDbContext dbContext, IEmailService emailService)
        {
            _dbContext = dbContext;
            _emailService = emailService;
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

    }
}

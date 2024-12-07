using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeMeal.Core.DTOs;
using RecipeMeal.Core.DTOs.Admin;
using RecipeMeal.Core.Interfaces.Services;
using System.Threading.Tasks;

namespace RecipeMeal.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Authorize(Roles = "Admin")]
	public class AdminController : ControllerBase
	{
		private readonly IAdminService _adminService;

		public AdminController(IAdminService adminService)
		{
			_adminService = adminService;
		}

		[HttpGet("users")]
		public async Task<IActionResult> GetAllUsers()
		{
			var users = await _adminService.GetAllUsersAsync();
			return Ok(users);
		}

		[HttpPost("add-user")]
		public async Task<IActionResult> AddUser([FromBody] UserDto userDto)
		{
			try
			{
				var message = await _adminService.AddUserAsync(userDto);
				return Ok(new { message });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpPut("update-role")]
		public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleDto dto)
		{
			try
			{
				var message = await _adminService.UpdateRoleAsync(dto);
				return Ok(new { message });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpPut("approve-user")]
		public async Task<IActionResult> ApproveUser([FromBody] ApproveUserDto dto)
		{
			try
			{
				var message = await _adminService.ApproveUserAsync(dto);
				return Ok(new { message });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpDelete("delete-user/{username}")]
		public async Task<IActionResult> DeleteUser(string username)
		{
			try
			{
				var message = await _adminService.DeleteUserAsync(username);
				return Ok(new { message });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}
	}
}

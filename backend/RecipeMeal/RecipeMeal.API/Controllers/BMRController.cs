using Microsoft.AspNetCore.Mvc;
using RecipeMeal.Core.DTOs;
using RecipeMeal.Core.DTOs.BMR;
using RecipeMeal.Core.Interfaces.Services;
using System;
using System.Threading.Tasks;

namespace RecipeMeal.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class BMRController : ControllerBase
	{
		private readonly IBMRCalculatorService _bmrCalculatorService;

		public BMRController(IBMRCalculatorService bmrCalculatorService)
		{
			_bmrCalculatorService = bmrCalculatorService;
		}

		[HttpPost("calculate-bmr")]
		public async Task<IActionResult> CalculateBMR([FromBody] BMRCalculatorDto dto)
		{
			try
			{
				if (dto == null)
				{
					return BadRequest(new { message = "Invalid input data." });
				}

				var bmrResult = await _bmrCalculatorService.CalculateBMRAsync(dto);
				return Ok(new { BMR = bmrResult });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}
	}
}

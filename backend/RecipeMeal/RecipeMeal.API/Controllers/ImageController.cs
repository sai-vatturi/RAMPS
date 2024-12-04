using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeMeal.Core.Interfaces;

namespace RecipeMeal.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ImageController : ControllerBase
	{
		private readonly IImageService _imageService;

		public ImageController(IImageService imageService)
		{
			_imageService = imageService;
		}

		[HttpPost("upload")]
		[Authorize] // Requires authenticated user
		public async Task<IActionResult> UploadFile(string description, DateTime clientDate, IFormFile file)
		{
			if (file == null)
				return BadRequest("File is required.");

			if (string.IsNullOrWhiteSpace(description))
				return BadRequest("Description is required.");

			// Process the file and upload it
			string imageUrl = await _imageService.UploadImageAsync(file);

			return Ok(new
			{
				Description = description,
				ClientDate = clientDate,
				ImageUrl = imageUrl
			});
		}
	}
}

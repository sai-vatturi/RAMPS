using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;


namespace RecipeMeal.API.Filters
{
	public class FileUploadOperationFilter : IOperationFilter
	{
		public void Apply(OpenApiOperation operation, OperationFilterContext context)
		{
			if (operation == null || context == null) return;

			// Check if the operation has an IFormFile parameter
			var hasFileUploadParameter = context.ApiDescription.ParameterDescriptions
				.Any(p => p.ModelMetadata?.ModelType == typeof(IFormFile));

			if (!hasFileUploadParameter) return;

			operation.RequestBody = new OpenApiRequestBody
			{
				Content = new System.Collections.Generic.Dictionary<string, OpenApiMediaType>
				{
					["multipart/form-data"] = new OpenApiMediaType
					{
						Schema = new OpenApiSchema
						{
							Type = "object",
							Properties = context.ApiDescription.ParameterDescriptions
								.ToDictionary(
									p => p.Name,
									p => p.ModelMetadata?.ModelType == typeof(IFormFile)
										? new OpenApiSchema { Type = "string", Format = "binary" }
										: new OpenApiSchema { Type = "string" }
								)
							}
						}
					}
				};
		}
	}
}

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RecipeMeal.API.Filters;
using RecipeMeal.Core.Interfaces;
using RecipeMeal.Core.Interfaces.Services;
using RecipeMeal.Core.Services;
using RecipeMeal.Infrastructure.Data;
using RecipeMeal.Infrastructure.Services;
using RecipeMeal.Infrastructure.Validators;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "RecipeMeal API", Version = "v1" });

	// Enable file upload
	c.OperationFilter<FileUploadOperationFilter>();

	// Add JWT Bearer Authentication definition to Swagger
	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		Scheme = "bearer",
		BearerFormat = "JWT",
		Description = "Enter 'Bearer' [space] and then your token below.\n\nExample: \"Bearer abcdef12345\""
	});

	c.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			Array.Empty<string>()
		}
	});
});

// Enable CORS for unrestricted access
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll", policy =>
	{
		policy.AllowAnyOrigin()   // Allow requests from any origin
			  .AllowAnyHeader()   // Allow any HTTP headers
			  .AllowAnyMethod();  // Allow any HTTP methods (GET, POST, etc.)
	});
});

// Serialize enums as strings in JSON
builder.Services.AddControllers()
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
	});

// Configure JSON serialization options to handle circular references
builder.Services.AddControllers()
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
		options.JsonSerializerOptions.WriteIndented = true;
	});

// Register application services
builder.Services.AddScoped<IImageService, AzureBlobStorageService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRecipeService, RecipeService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IBMRCalculatorService, BMRCalculatorService>();
builder.Services.AddScoped<INutritionService, NutritionService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IMealPlanService, MealPlanService>();
builder.Services.AddScoped<IShoppingService, ShoppingService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<UserValidationService>();
builder.Services.AddScoped<IFoodRecommendationService, FoodRecommendationService>();
builder.Services.AddScoped<IHomePageService, HomePageService>();
builder.Services.AddHttpClient();

// Configure Entity Framework and database context
builder.Services.AddDbContext<RecipeMealDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = builder.Configuration["Jwt:Issuer"],
			ValidAudience = builder.Configuration["Jwt:Audience"],
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]))
		};
	});

var app = builder.Build();

// Configure middleware pipeline

// Enable Swagger UI for all environments
app.UseSwagger();
app.UseSwaggerUI(c =>
{
	c.SwaggerEndpoint("/swagger/v1/swagger.json", "RecipeMeal API v1");
});

// Remove HTTPS redirection to use HTTP only
// app.UseHttpsRedirection();

// Enable CORS middleware
app.UseCors("AllowAll");

// Enable authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Map controllers
app.MapControllers();

// Ensure the app listens on all interfaces on port 80
app.Urls.Add("http://0.0.0.0:80");

// Run the application
app.Run();

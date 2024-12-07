using RecipeMeal.Infrastructure.Data;

namespace RecipeMeal.Infrastructure.Validators
{
	public class UserValidationService
	{
		private readonly RecipeMealDbContext _dbContext;

		public UserValidationService(RecipeMealDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<bool> EmailExistsAsync(string email)
		{
			return await Task.FromResult(_dbContext.Users.Any(u => u.Email == email));
		}

		public async Task<bool> UsernameExistsAsync(string username)
		{
			return await Task.FromResult(_dbContext.Users.Any(u => u.Username == username));
		}
	}
}

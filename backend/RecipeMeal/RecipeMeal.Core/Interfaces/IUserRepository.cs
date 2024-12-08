using RecipeMeal.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeMeal.Core.Interfaces
{
	public interface IUserRepository
	{
		Task<List<User>> GetAllUsersAsync();
		Task<User> GetByUsernameAsync(string username);
		Task<User> GetByEmailAsync(string email);
		Task<User> GetByPasswordResetTokenAsync(string token);
		Task<User> GetByEmailVerificationTokenAsync(string token);
		Task AddUserAsync(User user);
		Task UpdateUserAsync(User user);
		Task DeleteUserAsync(User user);
		
	}
}

using RecipeMeal.Core.DTOs;
using RecipeMeal.Core.DTOs.BMR;
using System.Threading.Tasks;

namespace RecipeMeal.Core.Interfaces.Services
{
	public interface IBMRCalculatorService
	{
		Task<double> CalculateBMRAsync(BMRCalculatorDto dto);
	}
}

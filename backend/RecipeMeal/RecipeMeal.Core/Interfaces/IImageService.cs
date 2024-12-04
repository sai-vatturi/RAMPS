using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace RecipeMeal.Core.Interfaces
{
    public interface IImageService
    {
        Task<string> UploadImageAsync(IFormFile file);
    }
}

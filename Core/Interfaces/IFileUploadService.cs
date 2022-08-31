using Microsoft.AspNetCore.Http;

namespace Core.Interfaces
{
    public interface IFileUploadService
    {
        Task<string> UploadFile(IFormFile file);
    }
}
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;


namespace Infrastructure.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IHostingEnvironment _environment;

        public FileUploadService(IHostingEnvironment environment)
        {

            _environment = environment;
        }

        public async Task<string> UploadFile(IFormFile file)
        {
            if (file.Length > 0)
            {
                var fileExtension = Path.GetExtension(file.FileName);
                string newName = string.Format(@"{0}{1}", Guid.NewGuid(), fileExtension);
                string path = "images/products/" + newName;
                using (var fileStream = new FileStream(_environment.WebRootPath + $"/{path}", FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                return path;
            }
            else
            {
                return null;
            }
        }
    }
}
using Microsoft.AspNetCore.Http;

namespace Identity.Service.Abstract
{
    public interface IImageService
    {
         Task<string> Add(IFormFile formFile);
    }
}
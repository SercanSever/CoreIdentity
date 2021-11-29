using System.Reflection.Metadata;
using Identity.Service.Abstract;
using Microsoft.AspNetCore.Http;

namespace Identity.Service.Concrete
{
   public class ImageManager : IImageService
   {
      public async Task<string> Add(IFormFile formFile)
      {
         var fileName = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);
         var path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/Upload/");
         var fullPath = Path.Combine(path, fileName);
         using (var stream = new FileStream(fullPath, FileMode.Create))
         {
            await formFile.CopyToAsync(stream);
         }
         return fileName;
      }
   }
}
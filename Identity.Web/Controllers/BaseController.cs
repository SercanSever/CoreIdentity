using System.Threading.Tasks;
using Identity.Entity;
using Identity.Service.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Web.Controllers
{
   public class BaseController : Controller
   {
      protected readonly UserManager<User> _userManager;
      protected readonly SignInManager<User> _signInManager;
      protected readonly ICommonService _commonService;
      protected readonly IImageService _imageService;
     
      public BaseController(UserManager<User> userManager, SignInManager<User> signInManager, ICommonService commonService, IImageService imageService)
      {
         _userManager = userManager;
         _signInManager = signInManager;
         _commonService = commonService;
         _imageService = imageService;
      }
      protected async Task<User> CurrentUser() => await _userManager.FindByNameAsync(User.Identity.Name);
      protected void AddModelError(IdentityResult result)
      {
         foreach (var error in result.Errors)
         {
            ModelState.AddModelError("", error.Description);
         }
      }

   }
}
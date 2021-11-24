using System.Threading.Tasks;
using Identity.Entity;
using Identity.Web.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Web.Controllers
{
   public class LoginController : Controller
   {
      private readonly UserManager<User> _userManager;
      public LoginController(UserManager<User> userManager)
      {
         _userManager = userManager;
      }

      #region Login
      public IActionResult SignIn()
      {
         return View();
      }
      [HttpPost]
      public async Task<IActionResult> SignIn(UserViewModel userViewModel)
      {
          if (ModelState.IsValid)
          {
              
          }
         return View();
      }
      #endregion

      #region Register
      public IActionResult SignUp()
      {
         return View();
      }
      [HttpPost]
      public async Task<IActionResult> SignUp(UserViewModel userViewModel)
      {
         if (ModelState.IsValid)
         {
            User user = new User();
            user.UserName = userViewModel.UserName;
            user.Email = userViewModel.Email;
            user.PhoneNumber = userViewModel.PhoneNumber;
            var result = await _userManager.CreateAsync(user, userViewModel.Password);
            if (result.Succeeded){
                return RedirectToAction("SignIn");
            }else{
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("",error.Description);
                }
            }
         }
         return View(userViewModel);
      }

      #endregion




   }
}
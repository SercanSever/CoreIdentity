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
      private readonly SignInManager<User> _signInManager;
      public LoginController(UserManager<User> userManager, SignInManager<User> signInManager)
      {
         _userManager = userManager;
         _signInManager = signInManager;
      }

      #region Login
      public IActionResult SignIn(string returnUrl)
      {
         TempData["ReturnUrl"] = returnUrl;
         return View();
      }
      [HttpPost]
      public async Task<IActionResult> SignIn(LoginViewModel loginViewModel)
      {
         if (ModelState.IsValid)
         {
            var user = await _userManager.FindByEmailAsync(loginViewModel.Email);
            if (user != null)
            {
               await _signInManager.SignOutAsync();
               var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, loginViewModel.RememberMe, false);
               if (result.Succeeded)
               {
                  if(TempData["ReturnUrl"] != null){
                     return Redirect(TempData["ReturnUrl"].ToString());
                  }
                  return RedirectToAction("Index", "Home");
               }
               else
               {
                  ModelState.AddModelError(nameof(LoginViewModel.Email), "There is no such user");
               }
               return View(loginViewModel);
            }
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
            if (result.Succeeded)
            {
               return RedirectToAction("SignIn");
            }
            else
            {
               foreach (var error in result.Errors)
               {
                  ModelState.AddModelError("", error.Description);
               }
            }
         }
         return View(userViewModel);
      }

      #endregion




   }
}
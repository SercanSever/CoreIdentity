using System.Threading;
using System;
using System.Threading.Tasks;
using Identity.Entity;
using Identity.Service.Abstract;
using Identity.Service.Enums;
using Identity.Service.Helpers.EmailHelper;
using Identity.Web.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Web.Controllers
{
   public class LoginController : Controller
   {
      private readonly UserManager<User> _userManager;
      private readonly SignInManager<User> _signInManager;
      private readonly ICommonService _commonService;
      public LoginController(UserManager<User> userManager, SignInManager<User> signInManager, ICommonService commonService)
      {
         _userManager = userManager;
         _signInManager = signInManager;
         _commonService = commonService;
      }

      #region Login
      public IActionResult SignIn(string returnUrl)
      {
         if (User.Identity.IsAuthenticated)
         {
            return RedirectToAction("Index", "Home");
         }
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
               if (await _userManager.IsLockedOutAsync(user))
               {
                  ModelState.AddModelError("", "Your account has been locked for a few minutes. Please try again later");
               }

               if (_userManager.IsEmailConfirmedAsync(user).Result == false)
               {
                  ModelState.AddModelError("", "Email address is not verified. Check your email.");
                  return View(loginViewModel);
               }


               await _signInManager.SignOutAsync();
               var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, loginViewModel.RememberMe, false);
               if (result.Succeeded)
               {
                  await _userManager.ResetAccessFailedCountAsync(user); //0

                  if (TempData["ReturnUrl"] != null)
                  {
                     return Redirect(TempData["ReturnUrl"].ToString());
                  }
                  return RedirectToAction("Index", "Home");
               }
               else
               {
                  await _userManager.AccessFailedAsync(user);
                  int fail = await _userManager.GetAccessFailedCountAsync(user);
                  if (fail == 3)
                  {
                     await _userManager.SetLockoutEndDateAsync(user, new DateTimeOffset(DateTime.Now.AddMinutes(10)));
                     ModelState.AddModelError("", "Account locked cause 3 fail login attempt. Please try 10 minutes later");
                  }
                  else
                  {
                     ModelState.AddModelError(nameof(LoginViewModel.Email), "There is no such user");
                  }
               }
            }
            else
            {
               ModelState.AddModelError(nameof(LoginViewModel.Email), "There is no such user");
            }
         }
         return View(loginViewModel);
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
               var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
               var link = Url.Action("ConfirmEmail","Login", new
               {
                  userId = user.Id,
                  token = confirmationToken
               }, protocol: HttpContext.Request.Scheme);
               EmailConfirm.SendEmailForConfirmation(link, user.Email);
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
      public async Task<IActionResult> ConfirmEmail(string userId, string token)
      {
         var user = await _userManager.FindByIdAsync(userId);
         var result = await _userManager.ConfirmEmailAsync(user, token);
         if (result.Succeeded)
         {
            ViewBag.confirm = _commonService.ShowAlert(Alerts.Success, "Email verified successfully");
         }
         else
         {
            ViewBag.confirm = _commonService.ShowAlert(Alerts.Success, "Email not verified");
         }
         return View();
      }

      #endregion

      #region ForgotPassword
      public IActionResult ForgotPassword()
      {
         return View();
      }
      [HttpPost]
      public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPasswordViewModel)
      {

         if (ModelState.IsValid)
         {
            var user = await _userManager.FindByEmailAsync(forgotPasswordViewModel.Email);
            if (user != null)
            {
               var passwordRestToken = await _userManager.GeneratePasswordResetTokenAsync(user);
               var passwordResetLink = Url.Action("ResetPasswordConfirm", "Login", new
               {
                  userId = user.Id,
                  token = passwordRestToken
               }, HttpContext.Request.Scheme);

               PasswordReset.PasswordResetSendEmail(passwordResetLink, user.Email);
               ViewBag.alert = _commonService.ShowAlert(Alerts.Success, "Please check your email.");
            }
            else
            {
               ModelState.AddModelError("", "There is no such email address");
            }
         }
         return View(forgotPasswordViewModel);
      }

      public IActionResult ResetPasswordConfirm(string userId, string token)
      {
         TempData["userId"] = userId;
         TempData["token"] = token;
         return View();
      }

      [HttpPost]
      public async Task<IActionResult> ResetPasswordConfirm(NewPasswordViewModel newPasswordViewModel)
      {
         var userId = TempData["userId"].ToString();
         var token = TempData["token"].ToString();

         User user = await _userManager.FindByIdAsync(userId);
         if (user != null)
         {
            var result = await _userManager.ResetPasswordAsync(user, token, newPasswordViewModel.Password);
            if (result.Succeeded)
            {
               await _userManager.UpdateSecurityStampAsync(user);
               ViewBag.PasswordResetSucces = _commonService.ShowAlert(Alerts.Success, "Your password is updated successfuly");
               Thread.Sleep(2000);
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
         else
         {
            ModelState.AddModelError("", "Something went wrong. User not found");
         }
         return View(newPasswordViewModel);
      }
      #endregion
   }
}
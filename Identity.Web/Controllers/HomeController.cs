using System.Threading;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Identity.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Identity.Entity;
using Mapster;
using Identity.Web.Models.ViewModels;
using Identity.Service.Abstract;
using Identity.Service.Enums;

namespace Identity.Web.Controllers
{
   [Authorize]
   public class HomeController : Controller
   {
      private readonly UserManager<User> _userManager;
      private readonly SignInManager<User> _signInManager;
      private readonly ICommonService _commonService;

      public HomeController(UserManager<User> userManager, SignInManager<User> signInManager, ICommonService commonService)
      {
         _userManager = userManager;
         _signInManager = signInManager;
         _commonService = commonService;
      }

      public async Task<IActionResult> Index()
      {
         var user = await _userManager.FindByNameAsync(User.Identity.Name);
         var userViewModel = user.Adapt<UserViewModel>();

         return View(userViewModel);
      }
      public IActionResult ChangePassword()
      {
         return View();
      }
      [HttpPost]
      public async Task<IActionResult> ChangePassword(PasswordChangeViewModel passwordChangeViewModel)
      {

         if (ModelState.IsValid)
         {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            bool oldPasswordConfirm = await _userManager.CheckPasswordAsync(user, passwordChangeViewModel.OldPassword);
            if (oldPasswordConfirm)
            {
               var result = await _userManager.ChangePasswordAsync(user, passwordChangeViewModel.OldPassword, passwordChangeViewModel.NewPassword);
               if (result.Succeeded)
               {
                  await _userManager.UpdateSecurityStampAsync(user);
                  await _signInManager.SignOutAsync();
                  await _signInManager.PasswordSignInAsync(user, passwordChangeViewModel.NewPassword, true, false);
                  ViewBag.alert="true";
                  ViewBag.ChangeSuccess = _commonService.ShowAlert(Alerts.Success, "Password changed successfully");
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
               ModelState.AddModelError("", "The old password is not correct");
            }
         }
         return View(passwordChangeViewModel);
      }

   }
}


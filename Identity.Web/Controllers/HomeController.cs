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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

namespace Identity.Web.Controllers
{
   [Authorize]
   public class HomeController : Controller
   {
      private readonly UserManager<User> _userManager;
      private readonly SignInManager<User> _signInManager;
      private readonly ICommonService _commonService;
      private readonly IImageService _imageService;

      public HomeController(UserManager<User> userManager, SignInManager<User> signInManager, ICommonService commonService,IImageService imageService)
      {
         _userManager = userManager;
         _signInManager = signInManager;
         _commonService = commonService;
         _imageService = imageService;
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
                  ViewBag.alert = "true";
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

      public async Task<IActionResult> UpdateUser()
      {
         ViewBag.gender = new SelectList(Enum.GetNames(typeof(Gender)));

         var user = await _userManager.FindByNameAsync(User.Identity.Name);
         var updateUserViewModel = user.Adapt<UpdateUserViewModel>();
         return View(updateUserViewModel);
      }
      [HttpPost]
      public async Task<IActionResult> UpdateUser(UpdateUserViewModel updateUserViewModel, IFormFile userImage)
      {
         if (ModelState.IsValid)
         {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (userImage != null && userImage.Length > 0)
            {
               user.Image = await _imageService.Add(userImage);
            }
            user.UserName = updateUserViewModel.UserName;
            user.Email = updateUserViewModel.Email;
            user.PhoneNumber = updateUserViewModel.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
               await _userManager.UpdateSecurityStampAsync(user);
               await _signInManager.SignOutAsync();
               await _signInManager.SignInAsync(user, true);
               ViewBag.alert = "true";
               ViewBag.updateSuccess = _commonService.ShowAlert(Alerts.Success, "User infos changed successfully");
            }
            else
            {
               foreach (var error in result.Errors)
               {
                  ModelState.AddModelError("", error.Description);
               }
            }
         }
         return View(updateUserViewModel);
      }
      public void Logout()
      {
         _signInManager.SignOutAsync();
      }
   }
}


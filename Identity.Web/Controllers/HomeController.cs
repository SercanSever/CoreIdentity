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
   public class HomeController : BaseController
   {


      public HomeController(UserManager<User> userManager, SignInManager<User> signInManager, ICommonService commonService, IImageService imageService) : base(userManager, signInManager, commonService, imageService)
      {

      }

      public async Task<IActionResult> Index()
      {
         var user = await CurrentUser();
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
            var user = await CurrentUser();
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
                  AddModelError(result);
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

         var user = await CurrentUser();
         var updateUserViewModel = user.Adapt<UpdateUserViewModel>();
         return View(updateUserViewModel);
      }
      [HttpPost]
      public async Task<IActionResult> UpdateUser(UpdateUserViewModel updateUserViewModel, IFormFile userImage)
      {
         if (ModelState.IsValid)
         {
            var user = await CurrentUser();

            if (userImage != null && userImage.Length > 0)
            {
               var fileName = await _imageService.Add(userImage);
               user.Image = fileName;
            }
            user.UserName = updateUserViewModel.UserName;
            user.Email = updateUserViewModel.Email;
            user.PhoneNumber = updateUserViewModel.PhoneNumber;
            user.City = updateUserViewModel.City;
            user.Gender = (int)updateUserViewModel.Gender;
            user.BirthDay = updateUserViewModel.BirthDay;

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
               AddModelError(result);
            }
         }
         return View(updateUserViewModel);
      }

      [Authorize("İstanbulPolicy")]
      public IActionResult CityClaim()
      {
         return View();
      }

      [Authorize("ViolencePolicy")]
      public IActionResult ViolenceClaim()
      {
         return View();
      }


      public IActionResult AccessDenied()
      {
         return View();
      }
      public void Logout()
      {
         _signInManager.SignOutAsync();
      }
   }
}


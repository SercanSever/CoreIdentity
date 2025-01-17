﻿using System.Threading;
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
using System.Security.Claims;

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
         var user = CurrentUser;
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
            var user = CurrentUser;
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

      public IActionResult UpdateUser()
      {
         ViewBag.gender = new SelectList(Enum.GetNames(typeof(Gender)));

         var user = CurrentUser;
         var updateUserViewModel = user.Adapt<UpdateUserViewModel>();
         return View(updateUserViewModel);
      }
      [HttpPost]
      public async Task<IActionResult> UpdateUser(UpdateUserViewModel updateUserViewModel, IFormFile userImage)
      {
         if (ModelState.IsValid)
         {
            var user = CurrentUser;

            string phone = _userManager.GetPhoneNumberAsync(user).Result;
            if (phone != updateUserViewModel.PhoneNumber)
            {
                if (_userManager.Users.Any(x=>x.PhoneNumber == updateUserViewModel.PhoneNumber))
                {
                    ModelState.AddModelError("","This phone number already exists");
                    return View(updateUserViewModel);
                }
            }

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

      public async Task<IActionResult> ExchangeRedirect()
      {
         bool result = User.HasClaim(x => x.Type == "ExpireDateExchange");

         if (!result)
         {
            Claim ExpireDateExchange = new Claim("ExpireDateExchange", DateTime.Now.AddDays(30).Date.ToShortDateString(), ClaimValueTypes.String, "Internal");

            await _userManager.AddClaimAsync(CurrentUser, ExpireDateExchange);

            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(CurrentUser, true);
         }

         return RedirectToAction("Exchange");
      }
      [Authorize("ExchangePolicy")]
      public IActionResult Exchange() //30 days trial page
      {
         return View();
      }


      public IActionResult AccessDenied(string returnUrl)
      {

         if (returnUrl.Contains("ViolenceClaim"))
         {
            ViewBag.message = "Access denied. You're not 18 yet.";
         }
         else if (returnUrl.Contains("CityClaim"))
         {
            ViewBag.message = "Access denied. You're not living in İstanbul.";
         }
         else if (returnUrl.Contains("ExchangePolicy"))
         {
            ViewBag.message = "Access denied. Trial is over.";
         }
         else
         {
            ViewBag.message = "Access denied.";
         }
         return View();
      }
      public void Logout()
      {
         _signInManager.SignOutAsync();
      }
   }
}



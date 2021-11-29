using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.DataAccess.Abstract;
using Identity.Entity;
using Identity.Service.Abstract;
using Identity.Web.Models.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Web.Controllers
{
   public class AdminController : BaseController
   {


      public AdminController(UserManager<User> userManager, RoleManager<Role> roleManager) : base(userManager, null, null, null, roleManager)
      {
      }
      public IActionResult Home()
      {
         var usersList = _userManager.Users.ToList();
         return View(usersList);
      }
      public IActionResult RoleCreate()
      {
         return View();
      }
      [HttpPost]
      public async Task<IActionResult> RoleCreate(RoleViewModel roleViewModel)
      {
         var role = new Role();
         role.Name = roleViewModel.Name;
         var result = await _roleManager.CreateAsync(role);
         if (result.Succeeded)
         {
            RedirectToAction("RoleCreate");
         }
         else
         {
            AddModelError(result);
         }

         return View();
      }
      public async Task<IActionResult> RoleDelete(string id)
      {
         var role = await _roleManager.FindByIdAsync(id);
         if (role != null)
         {
            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
               return RedirectToAction("Roles");
            }
            else
            {
               AddModelError(result);
            }
         }
         return RedirectToAction("Roles");
      }
      public async Task<IActionResult> RoleUpdate(string Id)
      {
         var role = await _roleManager.FindByIdAsync(Id);
         if (role != null)
         {
            return View(role.Adapt<RoleViewModel>());
         }
         return RedirectToAction("Roles");

      }
      [HttpPost]
      public async Task<IActionResult> RoleUpdate(RoleViewModel roleViewModel)
      {
         var role = await _roleManager.FindByIdAsync(roleViewModel.Id);
         if (role != null)
         {
            role.Name = roleViewModel.Name;
            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
               return RedirectToAction("Roles");
            }
            else
            {
               AddModelError(result);
            }
         }
         return RedirectToAction("RoleUpdate");
      }

      public IActionResult Roles()
      {
         var roleList = _roleManager.Roles.ToList();
         return View(roleList);
      }
      public async Task<IActionResult> AssignRole(string Id)
      {
         TempData["userId"] = Id;
         var user = await _userManager.FindByIdAsync(Id);
         ViewBag.userName = user.UserName;
         var roles = _roleManager.Roles.ToList();
         var userRoles = await _userManager.GetRolesAsync(user);

         List<AssignRoleViewModel> assignRolesViewModel = new List<AssignRoleViewModel>();

         foreach (var role in roles)
         {
            AssignRoleViewModel assignRole = new AssignRoleViewModel();
            assignRole.RoleId = role.Id;
            assignRole.RoleName = role.Name;
            if (userRoles.Contains(role.Name))
            {
               assignRole.Exist = true;
            }
            else
            {
               assignRole.Exist = false;
            }
            assignRolesViewModel.Add(assignRole);
         }
         return View(assignRolesViewModel);
      }
      [HttpPost]
      public async Task<IActionResult> AssignRole(List<AssignRoleViewModel> assignRoleViewModel)
      {
         var user = await _userManager.FindByIdAsync(TempData["userId"].ToString());
         foreach (var assignRole in assignRoleViewModel)
         {
            if (assignRole.Exist)
            {
                await _userManager.AddToRoleAsync(user,assignRole.RoleName);
            }else{
               await _userManager.RemoveFromRoleAsync(user,assignRole.RoleName);
            }
         }
         return RedirectToAction("Home");
      }
   }
}
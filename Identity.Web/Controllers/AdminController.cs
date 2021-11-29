using System.Linq;
using System.Threading.Tasks;
using Identity.DataAccess.Abstract;
using Identity.Entity;
using Identity.Service.Abstract;
using Identity.Web.Models.ViewModels;
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
      public IActionResult Roles()
      {
         return View();
      }
   }
}
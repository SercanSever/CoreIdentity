using System.Linq;
using Identity.DataAccess.Abstract;
using Identity.Entity;
using Identity.Service.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Web.Controllers
{
    public class AdminController : BaseController
    {


        public AdminController(UserManager<User> userManager,RoleManager<Role> roleManager) : base(userManager,null,null,null,roleManager)
        {
        }
        public IActionResult Home()
        {
            var usersList = _userManager.Users.ToList();
            return View(usersList);
        }
    }
}
using System.Linq;
using Identity.DataAccess.Abstract;
using Identity.Entity;
using Identity.Service.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;

        public AdminController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Home()
        {
            var usersList = _userManager.Users.ToList();
            return View(usersList);
        }
    }
}
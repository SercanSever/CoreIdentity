using Identity.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Web.Controllers
{
    public class LoginController : Controller
    {
          public IActionResult SignUp(){
            return View();
        }
        [HttpPost]
         public IActionResult SignUp(UserViewModel userModel){
            return View();
        }
    }
}
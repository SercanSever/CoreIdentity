using System.ComponentModel.DataAnnotations;
using Identity.Entity;

namespace Identity.Web.Models.ViewModels
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "Username is required." )]
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Email is required." )]
        [EmailAddress(ErrorMessage="E-mail is not in valid format")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required." )]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
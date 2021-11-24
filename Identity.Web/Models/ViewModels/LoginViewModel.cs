using System.ComponentModel.DataAnnotations;

namespace Identity.Web.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage="E-mail is not in valid format")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
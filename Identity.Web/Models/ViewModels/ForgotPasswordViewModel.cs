using System.ComponentModel.DataAnnotations;

namespace Identity.Web.Models.ViewModels
{
   public class ForgotPasswordViewModel
   {
      [Required]
      [EmailAddress(ErrorMessage = "E-mail is not in valid format")]
      public string Email { get; set; }

   }
}
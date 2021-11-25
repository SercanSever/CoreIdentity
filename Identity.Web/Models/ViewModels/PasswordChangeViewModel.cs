using System.ComponentModel.DataAnnotations;

namespace Identity.Web.Models.ViewModels
{
   public class PasswordChangeViewModel
   {
      [Required]
      [DataType(DataType.Password)]
      public string OldPassword { get; set; }
      [Required]
      [DataType(DataType.Password)]
      public string NewPassword { get; set; }
      [Required]
      [DataType(DataType.Password)]
      [Compare("NewPassword")]
      public string NewPasswordConfirm { get; set; }
   }
}
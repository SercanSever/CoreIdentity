using System.ComponentModel.DataAnnotations;

namespace Identity.Web.Models.ViewModels
{
   public class NewPasswordViewModel
   {
      [Required]
      [DataType(DataType.Password)]
      public string Password { get; set; }
   }
}
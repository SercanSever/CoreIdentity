using System;
using System.ComponentModel.DataAnnotations;
using Identity.Service.Enums;

namespace Identity.Web.Models.ViewModels
{
   public class UpdateUserViewModel
   {
      [Required]
      public string UserName { get; set; }
      [Required]
      [EmailAddress(ErrorMessage = "E-mail is not in valid format")]
      public string Email { get; set; }
      [Required]
      public string PhoneNumber { get; set; }
      [DataType(DataType.Date)]
      public DateTime BirthDay { get; set; }
      public Gender Gender { get; set; }
      public string Image { get; set; }
      public string City { get; set; }
   }
}
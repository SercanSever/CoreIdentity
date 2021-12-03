using System.Text.RegularExpressions;
using System;
using System.ComponentModel.DataAnnotations;
using Identity.Entity;
using Identity.Service.Enums;

namespace Identity.Web.Models.ViewModels
{
   public class UserViewModel
   {
      [Required(ErrorMessage = "Username is required.")]
      public string UserName { get; set; }
      [RegularExpression(@"^(05)([0-9]{2})\s?([0-9]{3})\s?([0-9]{2})\s?([0-9]{2})$", ErrorMessage = "Invalid phone number")]
      public string PhoneNumber { get; set; }
      [Required(ErrorMessage = "Email is required.")]
      [EmailAddress(ErrorMessage = "E-mail is not in valid format")]
      public string Email { get; set; }
      [Required(ErrorMessage = "Password is required.")]
      [DataType(DataType.Password)]
      public string Password { get; set; }
      [DataType(DataType.Date)]
      public DateTime BirthDay { get; set; }
      public Gender Gender { get; set; }
      public string Image { get; set; }
      public string City { get; set; }
   }
}
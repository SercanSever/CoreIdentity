using Identity.Entity;
using Microsoft.AspNetCore.Identity;

namespace Identity.Service.CustomValidation
{
   public class CustomePasswordValidator : IPasswordValidator<User>
   {
      public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user, string password)
      {
         List<IdentityError> errors = new List<IdentityError>();

         if (password.ToLower().Contains(user.UserName.ToLower()))
         {
            errors.Add(new IdentityError() { Code = "PasswordContainsUserName", Description = "Password cannot contain username" });
         }
         if (password.ToLower().Contains(user.Email.ToLower()))
         {
            errors.Add(new IdentityError() { Code = "PasswordContainsEmail", Description = "Password cannot contain email" });
         }
         if (password.ToLower().Contains("1234"))
         {
            errors.Add(new IdentityError() { Code = "PasswordContains1234", Description = "Password cannot contain consecutive numbers" });
         }
         if (errors.Count == 0)
         {
            return Task.FromResult(IdentityResult.Success);
         }
         else
         {
            return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
         }
      }
   }
}
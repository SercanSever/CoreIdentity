using Identity.Entity;
using Microsoft.AspNetCore.Identity;

namespace Identity.Service.CustomValidation
{
   public class CustomUserValidator : IUserValidator<User>
   {
      public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user)
      {
         List<IdentityError> errors = new List<IdentityError>();

         string[] _digits = new string[]{
             "0","1","2","3","4","5","6","7","8","9"
         };

         foreach (var digit in _digits)
         {
            if (user.UserName.StartsWith(digit))
            {
               errors.Add(new IdentityError() { Code = "UserNameStartsWithDigit", Description = "Username cannot start with digit" });
            }
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
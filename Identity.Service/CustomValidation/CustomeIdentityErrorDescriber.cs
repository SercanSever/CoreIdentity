using Microsoft.AspNetCore.Identity;

namespace Identity.Service.CustomValidation
{
   public class CustomeIdentityErrorDescriber : IdentityErrorDescriber
   {
      public override IdentityError InvalidUserName(string userName)
      {
         return new IdentityError()
         {
            Code = "InvalidUserName",
            Description = $"{userName} invalid."
         };
      }
      public override IdentityError DuplicateEmail(string email)
      {
         return new IdentityError()
         {
            Code = "DublicatedEmail",
            Description = $"{email} is already in use"
         };
      }
   }
}
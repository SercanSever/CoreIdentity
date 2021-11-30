using System.Security.Claims;
using Identity.Entity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Identity.Service.Concrete
{
   public class ClaimProvider : IClaimsTransformation
   {
      private readonly UserManager<User> _userManager;

      public ClaimProvider(UserManager<User> userManager)
      {
         _userManager = userManager;
      }

      public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
      {
         if (principal != null && principal.Identity.IsAuthenticated)
         {
            ClaimsIdentity identity = principal.Identity as ClaimsIdentity;
            var user = await _userManager.FindByNameAsync(identity.Name);

            if (user != null)
            {
               if (user.City != null)
               {
                  if (!principal.HasClaim(c => c.Type == "City"))
                  {
                     Claim cityClaim = new Claim("city", user.City, ClaimValueTypes.String, "Internal");
                     identity.AddClaim(cityClaim);
                  }
               }
            }
         }
         return principal;
      }
   }
}
using Identity.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Identity.Service.Helpers.CustomTagHelpers
{
   [HtmlTargetElement("td", Attributes = "user-roles")]
   public class UserRolesName : TagHelper
   {
      public UserManager<User> UserManager { get; set; }

      public UserRolesName(UserManager<User> userManager)
      {
         this.UserManager = userManager;
      }

      [HtmlAttributeName("user-roles")]
      public string Id { get; set; }

      public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
      {
         User user = await UserManager.FindByIdAsync(Id);

         IList<string> roles = await UserManager.GetRolesAsync(user);

         string html = string.Empty;

         roles.ToList().ForEach(x =>
         {
            html += $"<span class='badge badge-info'>  {x}  </span>";
         });

         output.Content.SetHtmlContent(html);
      }
   }
}
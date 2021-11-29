using Identity.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Identity.Service.Helpers.CustomTagHelpers
{
   [HtmlTargetElement("td", Attributes = "user-roles")]
   public class UserRoleName : TagHelper
   {
      public UserManager<User> UserManager {get;set;}
      public UserRoleName(UserManager<User> userManager)
      {
         UserManager = userManager;
      }
      [HtmlAttributeName("user-roles")]
      public string UserId { get; set; }

      public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
      {
         var user = await UserManager.FindByIdAsync(UserId);
         IList<string> roleList = await UserManager.GetRolesAsync(user);
         string html = string.Empty;
         roleList.ToList().ForEach(x => 
         {
            html += $"<span class='badge badge-info'>{x}</span>";
         });
         output.Content.SetHtmlContent(html);
      }
   }
}
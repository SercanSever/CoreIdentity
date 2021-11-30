using System.Text;
using Identity.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Identity.Service.Helpers.CustomTagHelpers
{
   [HtmlTargetElement("user-roles-tag-helper")]
   public class UserRolesName : TagHelper
   {
      [HtmlAttributeName("for-userId")]
      public string UserId { get; set; }
      public UserManager<User> UserManager { get; set; }
      public UserRolesName(UserManager<User> userManager)
      {
         this.UserManager = userManager;
      }

      public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
      {
         User user = await UserManager.FindByIdAsync(UserId);

         IList<string> roles = await UserManager.GetRolesAsync(user);

         output.TagMode = TagMode.StartTagAndEndTag;

         var sb = new StringBuilder();

         roles.ToList().ForEach(x =>
         {
            sb.AppendFormat("<span class='badge badge-info'>  {0}  </span>", x);
         });

         output.PreContent.SetHtmlContent(sb.ToString());
      }
   }
}
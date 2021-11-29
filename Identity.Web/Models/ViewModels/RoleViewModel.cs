using System.ComponentModel.DataAnnotations;

namespace Identity.Web.Models.ViewModels
{
    public class RoleViewModel
    {
        [Required(ErrorMessage="Role name is required")]
        public string Name { get; set; }
    }
}
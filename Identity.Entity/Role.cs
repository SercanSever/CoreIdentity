using Microsoft.AspNetCore.Identity;

namespace Identity.Entity
{
    public class Role : IdentityRole
    {
        public string? RoleType { get; set; }
    }
}
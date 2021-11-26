using Microsoft.AspNetCore.Identity;
namespace Identity.Entity
{
    public class User : IdentityUser
    {
        public string? City { get; set; }
        public string? Image { get; set; }
        public DateTime? BirthDay { get; set; }
        public int Gender { get; set; }
    }
}
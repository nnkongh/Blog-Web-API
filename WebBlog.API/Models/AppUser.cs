using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;

namespace WebBlog.API.Models
{
    public class AppUser : IdentityUser
    {
        public string Role { get; set; } = string.Empty;
    }
}

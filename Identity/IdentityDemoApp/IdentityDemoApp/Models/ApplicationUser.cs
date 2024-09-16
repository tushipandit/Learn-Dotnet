using Microsoft.AspNetCore.Identity;

namespace IdentityDemoApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string City { get; set; }
    }
}
